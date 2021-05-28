using System;
using System.Runtime.InteropServices;

namespace Spectre.Terminal
{
    internal static class TerminalFactory
    {
        public static ITerminal Create()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new Terminal(new WindowsDriver());
            }

            throw new PlatformNotSupportedException();
        }
    }
}
