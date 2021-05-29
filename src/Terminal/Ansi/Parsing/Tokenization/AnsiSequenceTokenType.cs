namespace Spectre.Terminal.Ansi
{
    internal enum AnsiSequenceTokenType
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
