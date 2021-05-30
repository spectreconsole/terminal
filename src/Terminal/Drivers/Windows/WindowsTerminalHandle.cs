using Microsoft.Windows.Sdk;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Spectre.Terminal.Windows
{
    internal abstract class WindowsTerminalHandle : IDisposable
    {
        public SafeHandle Handle { get; }
        public bool IsRedirected { get; }

        public WindowsTerminalHandle(STD_HANDLE_TYPE handle)
        {
            Handle = PInvoke.GetStdHandle_SafeHandle(handle);
            IsRedirected = !GetMode(out _) || (PInvoke.GetFileType(Handle) & 2) == 0;
        }

        public void Dispose()
        {
            Handle.Dispose();
        }

        public unsafe bool GetMode([NotNullWhen(true)] out CONSOLE_MODE? mode)
        {
            if (PInvoke.GetConsoleMode(Handle, out var result))
            {
                mode = result;
                return true;
            }

            mode = null;
            return false;
        }

        public unsafe bool AddMode(CONSOLE_MODE mode)
        {
            if (GetMode(out var currentMode))
            {
                return PInvoke.SetConsoleMode(Handle, currentMode.Value | mode);
            }

            return false;
        }

        public unsafe bool RemoveMode(CONSOLE_MODE mode)
        {
            if (GetMode(out var currentMode))
            {
                return PInvoke.SetConsoleMode(Handle, currentMode.Value & ~mode);
            }

            return false;
        }
    }
}
