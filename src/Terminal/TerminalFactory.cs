using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
