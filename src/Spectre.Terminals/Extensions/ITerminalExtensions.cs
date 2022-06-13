namespace Spectre.Terminals;

/// <summary>
/// Contains extension methods for <see cref="ITerminal"/>.
/// </summary>
public static partial class ITerminalExtensions
{
    /// <summary>
    /// Writes the specified buffer to the terminal's output handle.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="value">The value to write.</param>
    public static void Write(this ITerminal terminal, ReadOnlySpan<char> value)
    {
        terminal.Output.Write(value);
    }

    /// <summary>
    /// Writes the specified text to the terminal's output handle.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="value">The value to write.</param>
    public static void Write(this ITerminal terminal, string? value)
    {
        terminal.Output.Write(value);
    }

    /// <summary>
    /// Writes an empty line to the terminal's output handle.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    public static void WriteLine(this ITerminal terminal)
    {
        terminal.Output.WriteLine();
    }

    /// <summary>
    /// Writes the specified text followed by a line break to the terminal's output handle.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="value">The value to write.</param>
    public static void WriteLine(this ITerminal terminal, string? value)
    {
        terminal.Output.WriteLine(value);
    }
}
