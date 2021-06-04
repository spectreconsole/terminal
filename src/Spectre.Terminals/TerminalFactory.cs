using System;
using System.Runtime.InteropServices;
using Spectre.Terminals.Windows;

namespace Spectre.Terminals
{
    internal static class TerminalFactory
    {
        public static ITerminal Create()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new Terminal(new WindowsDriver(emulate: true));
            }

            throw new PlatformNotSupportedException();
        }
    }
}
