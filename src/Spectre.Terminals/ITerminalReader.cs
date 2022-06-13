namespace Spectre.Terminals;

/// <summary>
/// Represents a reader.
/// </summary>
public interface ITerminalReader
{
    /// <summary>
    /// Gets or sets the encoding.
    /// </summary>
    Encoding Encoding { get; set; }

    /// <summary>
    /// Gets a value indicating whether a key press is available in the input stream.
    /// </summary>
    bool IsKeyAvailable { get; }

    /// <summary>
    /// Gets a value indicating whether or not the reader has been redirected.
    /// </summary>
    bool IsRedirected { get; }

    /// <summary>
    /// Reads the next character from the standard input stream.
    /// </summary>
    /// <returns>
    /// The next character from the input stream, or negative one (-1)
    /// if there are currently no more characters to be read.
    /// </returns>
    int Read();

    /// <summary>
    /// Reads the next line of characters from the standard input stream.
    /// </summary>
    /// <returns>
    /// The next line of characters from the input stream, or null if
    /// no more lines are available.
    /// </returns>
    string? ReadLine();

    /// <summary>
    /// Obtains the next character or function key pressed by the user.
    /// </summary>
    /// <returns>
    /// An object that describes the System.ConsoleKey constant and Unicode character,
    /// if any, that correspond to the pressed console key. The System.ConsoleKeyInfo
    /// object also describes, in a bitwise combination of System.ConsoleModifiers values,
    /// whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously
    /// with the console key.
    /// </returns>
    ConsoleKeyInfo ReadKey();
}
