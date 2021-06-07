using System;
using System.Threading;
using Mono.Unix;
using Mono.Unix.Native;
using Syscall = Mono.Unix.Native.Syscall;

namespace Spectre.Terminals.Drivers
{
    internal abstract class PosixDriver : ITerminalDriver
    {
        private readonly Thread _signalListenerThread;
        private readonly ManualResetEvent _stopEvent;
        private readonly ManualResetEvent _stoppedEvent;

        public abstract string Name { get; }
        public bool IsRawMode { get; }
        public TerminalSize? Size { get; private set; }

        public event EventHandler<TerminalSignalEventArgs>? Signalled;

        public ITerminalReader Input { get; }
        public ITerminalWriter Output { get; }
        public ITerminalWriter Error { get; }

        protected PosixDriver()
        {
            Input = new PosixTerminalReader();
            Output = new PosixTerminalWriter(PosixConstants.STDIN);
            Error = new PosixTerminalWriter(PosixConstants.STDERR);

            Size = GetTerminalSize();

            _stopEvent = new ManualResetEvent(false);
            _stoppedEvent = new ManualResetEvent(false);
            _signalListenerThread = CreateSignalListener(_stopEvent);
            _signalListenerThread.Start();
        }

        public void Dispose()
        {
            if (!_stoppedEvent.WaitOne(0))
            {
                _stopEvent.Set();
                _signalListenerThread.Join();
                _stoppedEvent.WaitOne();
            }
        }

        public abstract bool EnableRawMode();
        public abstract bool DisableRawMode();
        public abstract void RefreshSettings();
        public abstract TerminalSize? GetTerminalSize();

        public bool EmitSignal(TerminalSignal signal)
        {
            switch (signal)
            {
                case TerminalSignal.SIGINT:
                    _ = Syscall.kill(0, Signum.SIGINT);
                    return true;
                case TerminalSignal.SIGQUIT:
                    _ = Syscall.kill(0, Signum.SIGQUIT);
                    return true;
            }

            return false;
        }

        private Thread CreateSignalListener(WaitHandle stop)
        {
            return new Thread(() =>
            {
                using var sigWinch = new UnixSignal(Signum.SIGWINCH);
                using var sigCont = new UnixSignal(Signum.SIGCONT);
                using var sigInt = new UnixSignal(Signum.SIGINT);
                using var sigQuit = new UnixSignal(Signum.SIGQUIT);

                var signals = new[] { sigWinch, sigCont, sigInt, sigQuit };
                while (!stop.WaitOne(0))
                {
                    var index = UnixSignal.WaitAny(signals, TimeSpan.FromMilliseconds(250));
                    if (index == 250)
                    {
                        continue;
                    }

                    if (index == -1)
                    {
                        break;
                    }

                    var signal = signals[index];
                    if (signal == stop)
                    {
                        break;
                    }

                    // If we are being restored from the background (SIGCONT), it is possible that
                    // terminal settings have been mangled, so restore them.
                    if (signal == sigCont)
                    {
                        RefreshSettings();
                    }

                    // Terminal width/height might have changed for SIGCONT, and will definitely
                    // have changed for SIGWINCH.
                    if (signal == sigCont || signal == sigWinch)
                    {
                        Size = GetTerminalSize();
                    }

                    if (signal == sigQuit || signal == sigInt)
                    {
                        if (Signalled != null)
                        {
                            // Propaate the signal
                            var received = signal == sigQuit ? TerminalSignal.SIGQUIT : TerminalSignal.SIGINT;
                            var signalEventArguments = new TerminalSignalEventArgs(received);
                            Signalled(null, new TerminalSignalEventArgs(received));

                            // Not cancelled?
                            if (!signalEventArguments.Cancel)
                            {
                                //// Get the value early to avoid ObjectDisposedException.
                                var num = ((UnixSignal)signal).Signum;

                                //// Remove our signal handler and send the signal again. Since we
                                //// have overwritten the signal handlers in CoreCLR and
                                //// System.Native, this gives those handlers an opportunity to run.
                                signal.Dispose();
                                Syscall.kill(Syscall.getpid(), num);
                            }
                        }
                    }
                }

                _stoppedEvent.Set();
            })
            {
                IsBackground = true,
                Name = "Signal Listener",
            };
        }
    }
}
