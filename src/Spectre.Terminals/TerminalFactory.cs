namespace Spectre.Terminals;

internal static class TerminalFactory
{
    public static ITerminal Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new Terminal(new WindowsDriver());
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return new Terminal(new LinuxDriver());
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return new Terminal(new MacOSDriver());
        }

        throw new PlatformNotSupportedException();
    }
}
