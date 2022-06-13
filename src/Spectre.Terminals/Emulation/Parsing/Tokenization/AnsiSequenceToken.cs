namespace Spectre.Terminals.Emulation;

internal sealed class AnsiSequenceToken
{
    public AnsiSequenceTokenType Type { get; }
    public ReadOnlyMemory<char> Content { get; set; }

    public char? AsCharacter()
    {
        return Content.Span[Content.Length - 1];
    }

    public AnsiSequenceToken(AnsiSequenceTokenType type, ReadOnlyMemory<char> value)
    {
        Type = type;
        Content = value;
    }
}
