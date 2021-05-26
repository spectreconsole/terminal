using System;

namespace Spectre.Terminal
{
    public static class TerminalReaderExtensions
    {
        public static byte? ReadRaw(this ITerminalReader reader)
        {
            Span<byte> span = stackalloc byte[1];
            return reader.Read(span) == 1 ? span[0] : null;
        }
    }
}
