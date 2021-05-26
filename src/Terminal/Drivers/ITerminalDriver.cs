namespace Spectre.Terminal
{
    public interface ITerminalDriver
    {
        TerminalCaps Caps { get; }

        ITerminalReader Input { get; }
        ITerminalWriter Output { get; }
        ITerminalWriter Error { get; }
    }
}
