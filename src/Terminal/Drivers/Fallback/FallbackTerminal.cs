using System;

namespace Spectre.Terminal
{
    public sealed class FallbackTerminal : ITerminalDriver
    {
        public TerminalCaps Caps { get; }

        public ITerminalReader Input { get; }
        public ITerminalWriter Output { get; }
        public ITerminalWriter Error { get; }

        public FallbackTerminal()
        {
            Caps = new TerminalCaps()
            {
                Ansi = false,
            };

            Input = new FallbackTerminalReader();
            Output = new FallbackTerminalWriter(() => Console.IsOutputRedirected, Console.Out.Write);
            Error = new FallbackTerminalWriter(() => Console.IsErrorRedirected, Console.Error.Write);
        }
    }
}
