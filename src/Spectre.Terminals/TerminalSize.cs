namespace Spectre.Terminals;

/// <summary>
/// Represents terminal size.
/// </summary>
[DebuggerDisplay("{Width}x{Height}")]
public readonly struct TerminalSize : IEquatable<TerminalSize>, IEqualityComparer<TerminalSize>
{
    /// <summary>
    /// Gets the terminal width in cells.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the terminal height in cells.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TerminalSize"/> struct.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public TerminalSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <inheritdoc/>
    public bool Equals(TerminalSize other)
    {
        return Width == other.Width
            && Height == other.Height;
    }

    /// <inheritdoc/>
    public bool Equals(TerminalSize x, TerminalSize y)
    {
        return x.Width == y.Width
            && x.Height == y.Height;
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] TerminalSize obj)
    {
        return HashCode.Combine(obj.Width, obj.Height);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Width}x{Height}";
    }
}
