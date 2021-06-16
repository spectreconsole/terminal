using System;
using System.Runtime.InteropServices;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminals.Drivers
{
    internal sealed class WindowsSignals : IDisposable
    {
        private readonly object _lock;
        private bool _installed;

        private EventHandler<TerminalSignalEventArgs>? _event;

        public event EventHandler<TerminalSignalEventArgs>? Signalled
        {
            add
            {
                _event = value;
                InstallHandler();
            }
            remove
            {
                UninstallHandler();
                _event = null;
            }
        }

        public WindowsSignals()
        {
            _lock = new object();
        }

        public void Dispose()
        {
            UninstallHandler();
        }

        public bool Emit(TerminalSignal signal)
        {
            uint? ctrlEvent = signal switch
            {
                TerminalSignal.SIGINT => WindowsConstants.Signals.CTRL_C_EVENT,
                TerminalSignal.SIGQUIT => WindowsConstants.Signals.CTRL_C_EVENT,
                _ => null,
            };

            if (ctrlEvent == null)
            {
                return false;
            }

            return PInvoke.GenerateConsoleCtrlEvent(ctrlEvent.Value, 0);
        }

        private BOOL Callback(uint ctrlType)
        {
            var @event = _event;

            TerminalSignal? signal = null;
            switch (ctrlType)
            {
                case WindowsConstants.Signals.CTRL_C_EVENT:
                    signal = TerminalSignal.SIGINT;
                    break;
                case WindowsConstants.Signals.CTRL_BREAK_EVENT:
                    signal = TerminalSignal.SIGQUIT;
                    break;
            }

            if (@event != null && signal != null)
            {
                var args = new TerminalSignalEventArgs(signal.Value);
                @event(null, args);
                return args.Cancel;
            }

            return false;
        }

        private void InstallHandler()
        {
            lock (_lock)
            {
                if (!_installed)
                {
                    if (!PInvoke.SetConsoleCtrlHandler(Callback, true))
                    {
                        var errorCode = Marshal.GetLastWin32Error();
                        throw new InvalidOperationException(
                            $"An error occured when installing Windows signal handler. Error code: {errorCode}");
                    }

                    _installed = true;
                }
            }
        }

        private void UninstallHandler()
        {
            lock (_lock)
            {
                if (_installed)
                {
                    if (!PInvoke.SetConsoleCtrlHandler(Callback, false))
                    {
                        var errorCode = Marshal.GetLastWin32Error();
                        throw new InvalidOperationException(
                            $"An error occured when uninstalling Windows signal handler. Error code: {errorCode}");
                    }

                    _installed = false;
                }
            }
        }
    }
}
