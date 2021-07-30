using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminals.Drivers
{
    internal abstract class WindowsTerminalHandle : IDisposable
    {
        private readonly object _lock;

        public SafeHandle Handle { get; set; }
        public bool IsRedirected { get; }

        protected WindowsTerminalHandle(STD_HANDLE_TYPE handle)
        {
            _lock = new object();

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
            lock (_lock)
            {
                if (GetMode(out var currentMode))
                {
                    return PInvoke.SetConsoleMode(Handle, currentMode.Value | mode);
                }

                return false;
            }
        }

        public unsafe bool RemoveMode(CONSOLE_MODE mode)
        {
            lock (_lock)
            {
                if (GetMode(out var currentMode))
                {
                    return PInvoke.SetConsoleMode(Handle, currentMode.Value & ~mode);
                }

                return false;
            }
        }
    }
}
