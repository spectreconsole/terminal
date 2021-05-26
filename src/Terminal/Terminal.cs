using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectre.Terminal
{
    public sealed class Terminal
    {
        public TerminalInput Input { get; }
        public TerminalOutput Output { get; }
        public TerminalOutput Error { get; }

        public Terminal(ITerminalDriver driver)
        {
            if (driver is null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            Input = new TerminalInput(driver.Input);
            Output = new TerminalOutput(driver.Output);
            Error = new TerminalOutput(driver.Error);
        }

        public static Terminal Create()
        {
            // Use the fallback terminal for now
            return new Terminal(new FallbackTerminal());
        }
    }
}
