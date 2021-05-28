using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Terminal.Ansi
{
    internal static class AnsiInstructionTokenizer
    {
        public static IReadOnlyList<AnsiInstructionToken> Tokenize(ReadOnlyMemory<char> buffer)
        {
            var result = new List<AnsiInstructionToken>();
            foreach (var (span, isEscapeCode) in AnsiSequenceSplitter.Split(buffer))
            {
                if (isEscapeCode)
                {
                    var tokens = TokenizeEscapeCode(new TextBuffer(span));
                    if (tokens.Count > 0)
                    {
                        result.Add(AnsiInstructionToken.Sequence(tokens));
                    }
                }
                else
                {
                    result.Add(AnsiInstructionToken.Text(span));
                }
            }

            return result;
        }

        private static IReadOnlyList<AnsiSequenceToken> TokenizeEscapeCode(TextBuffer buffer)
        {
            var result = new List<AnsiSequenceToken>();
            while (buffer.CanRead)
            {
                if (!ReadEscapeCodeToken(buffer, out var token))
                {
                    // Could not parse, so return an empty result
                    return Array.Empty<AnsiSequenceToken>();
                }

                result.Add(token);
            }

            return result;
        }

        private static bool ReadEscapeCodeToken(TextBuffer buffer, [NotNullWhen(true)] out AnsiSequenceToken? token)
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

            if (char.IsNumber(current))
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

            if (char.IsLetter(current))
            {
                var start = buffer.Position;
                buffer.Discard();
                token = new AnsiSequenceToken(
                    AnsiSequenceTokenType.Character,
                    buffer.Slice(start, start + 1));

                return true;
            }

            token = null;
            return false;
        }
    }
}
