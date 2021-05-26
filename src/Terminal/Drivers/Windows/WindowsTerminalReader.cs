using System;
using System.Text;
using Spectre.Terminal;
using Microsoft.Windows.Sdk;
using System.Runtime.InteropServices;

namespace Spectre.Terminal
{
    internal sealed class WindowsTerminalReader : WindowsTerminalHandle, ITerminalReader
    {
        public Encoding Encoding { get; }

        public WindowsTerminalReader()
            : base(STD_HANDLE_TYPE.STD_INPUT_HANDLE)
        {
            Encoding = EncodingHelper.GetEncodingFromCodePage((int)PInvoke.GetConsoleCP());
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
                    throw new InvalidOperationException("Could not write to buffer");
            }

            return (int)result;
        }
    }

    internal static class WindowsConstants
    {
        public const int ERROR_HANDLE_EOF = 38;
        public const int ERROR_BROKEN_PIPE = 109;
        public const int ERROR_NO_DATA = 232;
    }
}
