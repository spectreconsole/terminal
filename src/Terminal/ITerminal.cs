using System;

namespace Spectre.Terminal
{
    public interface ITerminal : IDisposable
    {
        TerminalInput Input { get; }
        TerminalOutput Output { get; }
        TerminalOutput Error { get; }

        bool EnableRawMode();
        bool DisableRawMode();
    }
}
