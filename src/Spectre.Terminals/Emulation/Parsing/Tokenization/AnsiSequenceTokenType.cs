namespace Spectre.Terminals.Emulation
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
