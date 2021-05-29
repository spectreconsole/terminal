using System;
using System.Runtime.InteropServices;
using Spectre.Terminal.Drivers;

namespace Spectre.Terminal
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
