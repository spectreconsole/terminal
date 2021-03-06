namespace Spectre.Terminals;

/// <summary>
/// Represents a writer.
/// </summary>
public interface ITerminalWriter
{
    /// <summary>
    /// Gets or sets the encoding.
    /// </summary>
    Encoding Encoding { get; set; }

    /// <summary>
    /// Gets a value indicating whether or not the writer has been redirected.
    /// </summary>
    bool IsRedirected { get; }

    /// <summary>
    /// Writes a sequence of bytes to the current writer.
    /// </summary>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the writer.</param>
    void Write(ReadOnlySpan<byte> buffer);
}
