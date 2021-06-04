using System;

namespace Spectre.Terminals
{
    public sealed class TerminalSignalEventArgs : EventArgs
    {
        public TerminalSignal Signal { get; }
        public bool Cancel { get; set; }

        public TerminalSignalEventArgs(TerminalSignal signal)
        {
            Signal = signal;
        }
    }
}
