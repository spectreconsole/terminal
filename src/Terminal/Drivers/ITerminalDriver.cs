using System;

namespace Spectre.Terminal
{
    public interface ITerminalDriver : IDisposable
    {
        bool SupportsAnsi { get; }

        ITerminalReader Input { get; }
        ITerminalWriter Output { get; }
        ITerminalWriter Error { get; }

        bool EnableRawMode();
        bool DisableRawMode();
    }
}
