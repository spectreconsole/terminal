using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminal.Windows
{
    internal class WindowsTerminalState
    {
        public SafeHandle Handle { get; }
        public ITerminalWriter Writer { get; }
        public Encoding Encoding => Writer.Encoding;

        public COORD? StoredCursorPosition { get; set; }

        public WindowsTerminalState(SafeHandle handle, ITerminalWriter writer)
        {
            Handle = handle ?? throw new ArgumentNullException(nameof(handle));
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }
    }
}