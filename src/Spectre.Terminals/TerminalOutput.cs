namespace Spectre.Terminals;

/// <summary>
/// Represents a mechanism to write to a terminal output handle.
/// </summary>
public sealed class TerminalOutput
{
    private readonly ITerminalWriter _writer;
    private readonly object _lock;
    private ITerminalWriter? _redirected;

    /// <summary>
    /// Gets or sets the encoding.
    /// </summary>
    public Encoding Encoding
    {
        get => GetEncoding();
        set => SetEncoding(value);
    }

    /// <summary>
    /// Gets a value indicating whether or not output has been redirected.
    /// </summary>
    public bool IsRedirected => GetIsRedirected();

    /// <summary>
    /// Initializes a new instance of the <see cref="TerminalOutput"/> class.
    /// </summary>
    /// <param name="writer">The terminal writer.</param>
    public TerminalOutput(ITerminalWriter writer)
    {
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        _lock = new object();
    }

    /// <summary>
    /// Redirects input to the specified <see cref="ITerminalWriter"/>.
    /// </summary>
    /// <param name="writer">
    /// The writer to redirect to,
    /// or <c>null</c>, if the current redirected writer should be removed.
    /// </param>
    public void Redirect(ITerminalWriter? writer)
    {
        lock (_lock)
        {
            _redirected = writer;
        }
    }

    /// <summary>
    /// Writes the specified buffer.
    /// </summary>
    /// <param name="value">The value to write.</param>
    public void Write(ReadOnlySpan<char> value)
    {
        lock (_lock)
        {
#if NET5_0_OR_GREATER
            var len = Encoding.GetByteCount(value);
            var array = ArrayPool<byte>.Shared.Rent(len);

            try
            {
                var span = array.AsSpan(0, len);
                Encoding.GetBytes(value, span);
                GetWriter().Write(span);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
#else
            var chars = value.ToArray();
            var bytes = Encoding.GetBytes(chars);
            GetWriter().Write(new Span<byte>(bytes));
#endif
        }
    }

    private Encoding GetEncoding()
    {
        lock (_lock)
        {
            return GetWriter().Encoding;
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
            GetWriter().Encoding = encoding;
        }
    }

    private bool GetIsRedirected()
    {
        lock (_lock)
        {
            return GetWriter().IsRedirected;
        }
    }

    private ITerminalWriter GetWriter()
    {
        return _redirected ?? _writer;
    }
}
