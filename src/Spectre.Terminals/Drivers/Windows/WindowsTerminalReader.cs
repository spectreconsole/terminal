using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminals.Windows
{
    internal sealed class WindowsTerminalReader : WindowsTerminalHandle, ITerminalReader
    {
        private readonly WindowsDriver _driver;
        private Encoding _encoding;

        public string Name { get; } = "STDIN";

        public Encoding Encoding
        {
            get => _encoding;
            set => SetEncoding(value);
        }

        public bool IsRawMode => _driver.IsRawMode;

        public WindowsTerminalReader(WindowsDriver driver)
            : base(STD_HANDLE_TYPE.STD_INPUT_HANDLE)
        {
            _encoding = EncodingHelper.GetEncodingFromCodePage((int)PInvoke.GetConsoleCP());
            _driver = driver;
        }

        public unsafe int Read(Span<byte> buffer)
        {
            uint result;
            uint* ptrResult = &result;

            fixed (byte* p = buffer)
            {
                if (PInvoke.ReadFile(Handle, p, (uint)buffer.Length, ptrResult, null))
                {
                    return (int)result;
                }
            }

            var error = Marshal.GetLastWin32Error();
            switch (error)
            {
                case WindowsConstants.ERROR_HANDLE_EOF:
                case WindowsConstants.ERROR_BROKEN_PIPE:
                case WindowsConstants.ERROR_NO_DATA:
                    break;
                default:
                    throw new InvalidOperationException($"Could not read from {Name}");
            }

            return (int)result;
        }

        private void SetEncoding(Encoding encoding)
        {
            if (PInvoke.SetConsoleCP((uint)encoding.CodePage))
            {
                _encoding = encoding;
            }
        }
    }
}
