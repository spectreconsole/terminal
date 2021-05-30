using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminals.Windows
{
    internal sealed class WindowsTerminalWriter : WindowsTerminalHandle, IWindowsTerminalWriter
    {
        public Encoding Encoding { get; }

        public WindowsTerminalWriter(STD_HANDLE_TYPE handle)
            : base(handle)
        {
            Encoding = EncodingHelper.GetEncodingFromCodePage((int)PInvoke.GetConsoleOutputCP());
        }

        public unsafe void Write(ReadOnlySpan<byte> buffer)
        {
            Write(Handle, buffer);
        }

        public unsafe void Write(SafeHandle handle, ReadOnlySpan<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                return;
            }

            uint written;
            uint* ptrWritten = &written;

            fixed (byte* ptrData = buffer)
            {
                if (PInvoke.WriteFile(handle, ptrData, (uint)buffer.Length, ptrWritten, null))
                {
                    return;
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
        }
    }
}
