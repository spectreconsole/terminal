using System;
using System.Runtime.InteropServices;

namespace Spectre.Terminal
{
    internal class WindowsTerminalState
    {
        public SafeHandle Handle { get; }
        public ITerminalWriter Writer { get; }

        public WindowsTerminalState(SafeHandle handle, ITerminalWriter writer)
        {
            Handle = handle ?? throw new ArgumentNullException(nameof(handle));
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }
    }
}