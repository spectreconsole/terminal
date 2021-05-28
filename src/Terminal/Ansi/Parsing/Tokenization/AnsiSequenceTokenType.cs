namespace Spectre.Terminal.Ansi
{
    public enum AnsiSequenceTokenType
    {
        Unknown,
        Csi,
        Character,
        Integer,
        Delimiter,
        Bang,
        Query,
        Equals,
        Send,
    }
}
