using System;
using System.Runtime.InteropServices;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminal
{
    internal class WindowsTerminalState
    {
        public SafeHandle Handle { get; }
        public ITerminalWriter Writer { get; }

        public COORD? StoredCursorPosition { get; set; }

        public WindowsTerminalState(SafeHandle handle, ITerminalWriter writer)
        {
            Handle = handle ?? throw new ArgumentNullException(nameof(handle));
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }
    }
}