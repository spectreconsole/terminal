using System;

namespace Spectre.Terminal
{
    public interface ITerminalDriver : IDisposable
    {
        string Name { get; }
        bool IsRawMode { get; }

        ITerminalReader Input { get; }
        ITerminalWriter Output { get; }
        ITerminalWriter Error { get; }

        bool EnableRawMode();
        bool DisableRawMode();
    }
}