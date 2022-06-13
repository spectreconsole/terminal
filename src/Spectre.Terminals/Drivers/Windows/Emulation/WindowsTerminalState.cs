namespace Spectre.Terminals.Drivers;

internal class WindowsTerminalState
{
    public IWindowsTerminalWriter Writer { get; }
    public SafeHandle Handle => AlternativeBuffer ?? MainBuffer;
    public Encoding Encoding => Writer.Encoding;

    public COORD? StoredCursorPosition { get; set; }

    public SafeHandle MainBuffer { get; set; }
    public SafeHandle? AlternativeBuffer { get; set; }

    public WindowsColors Colors { get; }

    public WindowsTerminalState(IWindowsTerminalWriter writer, WindowsColors colors)
    {
        MainBuffer = writer.Handle;
        Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        Colors = colors ?? throw new ArgumentNullException(nameof(colors));
    }
}