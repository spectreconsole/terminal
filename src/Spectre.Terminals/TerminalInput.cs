namespace Spectre.Terminals;

/// <summary>
/// Represents a mechanism to read input from the terminal.
/// </summary>
public sealed class TerminalInput
{
    private readonly ITerminalReader _reader;
    private readonly object _lock;
    private ITerminalReader? _redirected;

    /// <summary>
    /// Gets or sets the encoding.
    /// </summary>
    public Encoding Encoding
    {
        get => GetEncoding();
        set => SetEncoding(value);
    }

    /// <summary>
    /// Gets a value indicating whether or not input has been redirected.
    /// </summary>
    public bool IsRedirected => GetIsRedirected();

    /// <summary>
    /// Gets a value indicating whether a key press is available in the input stream.
    /// </summary>
    public bool IsKeyAvailable => throw new NotSupportedException();

    /// <summary>
    /// Initializes a new instance of the <see cref="TerminalInput"/> class.
    /// </summary>
    /// <param name="reader">The terminal reader.</param>
    public TerminalInput(ITerminalReader reader)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _lock = new object();
    }

    /// <summary>
    /// Redirects input to the specified <see cref="ITerminalReader"/>.
    /// </summary>
    /// <param name="reader">
    /// The reader to redirect to,
    /// or <c>null</c>, if the current redirected reader should be removed.
    /// </param>
    public void Redirect(ITerminalReader? reader)
    {
        lock (_lock)
        {
            _redirected = reader;
        }
    }

    /// <summary>
    /// Reads the next character from the standard input stream.
    /// </summary>
    /// <returns>
    /// The next character from the input stream, or negative one (-1)
    /// if there are currently no more characters to be read.
    /// </returns>
    public int Read()
    {
        return _reader.Read();
    }

    /// <summary>
    /// Reads the next line of characters from the standard input stream.
    /// </summary>
    /// <returns>
    /// The next line of characters from the input stream, or null if
    /// no more lines are available.
    /// </returns>
    public string? ReadLine()
    {
        return _reader.ReadLine();
    }

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
    public ConsoleKeyInfo ReadKey()
    {
        return _reader.ReadKey();
    }

    private bool GetIsRedirected()
    {
        lock (_lock)
        {
            return GetReader().IsRedirected;
        }
    }

    private Encoding GetEncoding()
    {
        lock (_lock)
        {
            return GetReader().Encoding;
        }
    }

    private void SetEncoding(Encoding encoding)
    {
        if (encoding is null)
        {
            throw new ArgumentNullException(nameof(encoding));
        }

        lock (_lock)
        {
            GetReader().Encoding = encoding;
        }
    }

    private ITerminalReader GetReader()
    {
        return _redirected ?? _reader;
    }
}
