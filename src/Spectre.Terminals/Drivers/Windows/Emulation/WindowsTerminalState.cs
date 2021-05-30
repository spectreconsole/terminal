using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminals.Windows.Emulation
{
    internal class WindowsTerminalState
    {
        public IWindowsTerminalWriter Writer { get; }
        public SafeHandle Handle => AlternativeBuffer ?? MainBuffer;
        public Encoding Encoding => Writer.Encoding;

        public COORD? StoredCursorPosition { get; set; }

        public SafeHandle MainBuffer { get; set; }
        public SafeHandle? AlternativeBuffer { get; set; }

        public WindowsTerminalState(IWindowsTerminalWriter writer)
        {
            MainBuffer = writer.Handle;
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }
    }
}