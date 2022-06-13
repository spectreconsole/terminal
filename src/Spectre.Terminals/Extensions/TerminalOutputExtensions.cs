namespace Spectre.Terminals;

/// <summary>
/// Contains extension methods for <see cref="TerminalOutput"/>.
/// </summary>
public static class TerminalOutputExtensions
{
    /// <summary>
    /// Writes the specified text.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value to write.</param>
    public static void Write(this TerminalOutput writer, string? value)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        writer.Write(value.AsSpan());
    }

    /// <summary>
    /// Writes an empty line.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public static void WriteLine(this TerminalOutput writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        Write(writer, Environment.NewLine);
    }

    /// <summary>
    /// Writes the specified text followed by a line break.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value to write.</param>
    public static void WriteLine(this TerminalOutput writer, string? value)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        Write(writer, value);
        Write(writer, Environment.NewLine);
    }
}
