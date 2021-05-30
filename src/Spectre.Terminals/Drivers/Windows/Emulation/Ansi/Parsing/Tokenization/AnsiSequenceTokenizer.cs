using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Terminals.Windows.Emulation
{
    internal static class AnsiSequenceTokenizer
    {
        public static IReadOnlyList<AnsiSequenceToken> Tokenize(MemoryCursor buffer)
        {
            var result = new List<AnsiSequenceToken>();
            while (buffer.CanRead)
            {
                if (!ReadSequenceToken(buffer, out var token))
                {
                    // Could not parse, so return an empty result
                    return Array.Empty<AnsiSequenceToken>();
                }

                result.Add(token);
            }

            return result;
        }

        private static bool ReadSequenceToken(MemoryCursor buffer, [NotNullWhen(true)] out AnsiSequenceToken? token)
        {
            var current = buffer.PeekChar();

            // ESC?
            if (current == 0x1b)
            {
                // CSI?
                var start = buffer.Position;
                buffer.Discard();
                if (buffer.CanRead && buffer.PeekChar() == '[')
                {
                    buffer.Discard();
                    token = new AnsiSequenceToken(AnsiSequenceTokenType.Csi, buffer.Slice(start, buffer.Position));
                    return true;
                }

                // Unknown escape sequence
                token = null;
                return false;
            }
            else if (char.IsNumber(current))
            {
                var start = buffer.Position;
                while (buffer.CanRead)
                {
                    current = buffer.PeekChar();
                    if (!char.IsNumber(current))
                    {
                        break;
                    }

                    buffer.Discard();
                }

                var end = buffer.Position;

                token = new AnsiSequenceToken(
                    AnsiSequenceTokenType.Integer,
                    buffer.Slice(start, end));

                return true;
            }
            else if (char.IsLetter(current))
            {
                var start = buffer.Position;
                buffer.Discard();
                token = new AnsiSequenceToken(
                    AnsiSequenceTokenType.Character,
                    buffer.Slice(start, start + 1));

                return true;
            }
            else if (current == ';')
            {
                var start = buffer.Position;
                buffer.Discard();
                token = new AnsiSequenceToken(
                    AnsiSequenceTokenType.Delimiter,
                    buffer.Slice(start, start + 1));

                return true;
            }
            else if (current == '?')
            {
                var start = buffer.Position;
                buffer.Discard();
                token = new AnsiSequenceToken(
                    AnsiSequenceTokenType.Query,
                    buffer.Slice(start, start + 1));

                return true;
            }

            token = null;
            return false;
        }
    }
}
