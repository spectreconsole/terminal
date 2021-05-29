using System;
using System.Text;
using Microsoft.Windows.Sdk;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Terminal.Drivers
{
    internal interface IWindowsTerminalWriter : ITerminalWriter, IDisposable
    {
        SafeHandle Handle { get; }

        bool GetMode([NotNullWhen(true)] out CONSOLE_MODE? mode);
        bool AddMode(CONSOLE_MODE mode);
        bool RemoveMode(CONSOLE_MODE mode);
    }

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
            if (buffer.IsEmpty)
            {
                return;
            }

            uint written;
            uint* ptrWritten = &written;

            fixed (byte* ptrData = buffer)
            {
                if (PInvoke.WriteFile(Handle, ptrData, (uint)buffer.Length, ptrWritten, null))
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
