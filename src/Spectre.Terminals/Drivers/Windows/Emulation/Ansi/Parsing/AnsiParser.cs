using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Spectre.Terminals.Windows.Emulation
{
    internal sealed class AnsiParser
    {
        public static IEnumerable<AnsiInstruction> Parse(ReadOnlyMemory<char> buffer)
        {
            foreach (var token in Tokenize(buffer))
            {
                if (token.IsText)
                {
                    yield return new PrintText(token.Text);
                }
                else if (token.IsSequence)
                {
                    var instruction = ParseInstruction(token.Sequence.ToArray());
                    if (instruction != null)
                    {
                        yield return instruction;
                    }
                }
            }
        }

        private static IReadOnlyList<AnsiToken> Tokenize(ReadOnlyMemory<char> buffer)
        {
            var result = new List<AnsiToken>();
            foreach (var (span, isSequence) in AnsiSequenceSplitter.Split(buffer))
            {
                if (isSequence)
                {
                    var tokens = AnsiSequenceTokenizer.Tokenize(new MemoryCursor(span));
                    if (tokens.Count > 0)
                    {
                        result.Add(AnsiToken.CreateSequence(tokens));
                    }
                }
                else
                {
                    result.Add(AnsiToken.CreateText(span));
                }
            }

            return result;
        }

        private static AnsiInstruction? ParseInstruction(AnsiSequenceToken[] tokens)
        {
            // Ignore empty sequences or non-CSI sequences (for now)
            if (tokens.Length == 0 || tokens[0].Type != AnsiSequenceTokenType.Csi)
            {
                return null;
            }

            var terminal = tokens[^1].AsCharacter();
            if (terminal == null)
            {
                return null;
            }

            // Get the parameters
            var parameters = new Span<AnsiSequenceToken>(tokens)[1..^1];

            // Query?
            if (IsQuery(parameters))
            {
                parameters = new Span<AnsiSequenceToken>(tokens)[2..^1];
                return terminal.Value switch
                {
                    'h' => ParseIntegerInstruction(parameters, value =>
                    {
                        if (value == 25)
                        {
                            return new ShowCursor();
                        }
                        else if (value == 1049)
                        {
                            return new EnableAlternativeBuffer();
                        }

                        return null;
                    }),
                    'l' => ParseIntegerInstruction(parameters, value =>
                    {
                        if (value == 25)
                        {
                            return new HideCursor();
                        }
                        else if (value == 1049)
                        {
                            return new DisableAlternativeBuffer();
                        }

                        return null;
                    }),
                    _ => null, // Unknown query instruction
                };
            }

            return terminal.Value switch
            {
                'A' => ParseIntegerInstruction(parameters, count => new CursorUp(count), defaultValue: 1),
                'B' => ParseIntegerInstruction(parameters, count => new CursorDown(count), defaultValue: 1),
                'C' => ParseIntegerInstruction(parameters, count => new CursorForward(count), defaultValue: 1),
                'D' => ParseIntegerInstruction(parameters, count => new CursorBack(count), defaultValue: 1),
                'E' => ParseIntegerInstruction(parameters, count => new CursorNextLine(count), defaultValue: 1),
                'F' => ParseIntegerInstruction(parameters, count => new CursorPreviousLine(count), defaultValue: 1),
                'G' => ParseIntegerInstruction(parameters, count => new CursorHorizontalAbsolute(count), defaultValue: 1),
                'H' => ParseCursorPosition(parameters),
                'J' => ParseIntegerInstruction(parameters, count => new EraseInDisplay(count), defaultValue: 0),
                'K' => ParseIntegerInstruction(parameters, count => new EraseInLine(count), defaultValue: 0),
                's' => new StoreCursor(),
                'u' => new RestoreCursor(),
                _ => null, // Unknown instruction
            };
        }

        private static bool IsQuery(ReadOnlySpan<AnsiSequenceToken> tokens)
        {
            return tokens.Length > 1 && tokens[0].Type == AnsiSequenceTokenType.Query;
        }

        private static AnsiInstruction? ParseIntegerInstruction(ReadOnlySpan<AnsiSequenceToken> tokens, Func<int, AnsiInstruction?> func, int defaultValue = 1)
        {
            if (tokens.Length != 1)
            {
                return func(defaultValue);
            }

            if (tokens[0].Type != AnsiSequenceTokenType.Integer)
            {
                return null;
            }

            return func(int.Parse(tokens[0].Content.Span));
        }

        private static AnsiInstruction? ParseIntegerInstruction(
            ReadOnlySpan<AnsiSequenceToken> tokens,
            Func<int, bool> predicate,
            Func<AnsiInstruction> func)
        {
            if (tokens[0].Type != AnsiSequenceTokenType.Integer)
            {
                return null;
            }

            if (predicate(int.Parse(tokens[0].Content.Span)))
            {
                return func();
            }

            return null;
        }

        private static CursorPosition? ParseCursorPosition(ReadOnlySpan<AnsiSequenceToken> tokens)
        {
            if (tokens.Length == 3)
            {
                if (IsSequence(tokens, AnsiSequenceTokenType.Integer, AnsiSequenceTokenType.Delimiter, AnsiSequenceTokenType.Integer))
                {
                    // [ROW];[COLUMN]H
                    return new CursorPosition(
                        int.Parse(tokens[2].Content.Span, provider: CultureInfo.InvariantCulture),
                        int.Parse(tokens[0].Content.Span, provider: CultureInfo.InvariantCulture));
                }
            }
            else if (tokens.Length == 2)
            {
                if (IsSequence(tokens, AnsiSequenceTokenType.Integer, AnsiSequenceTokenType.Delimiter))
                {
                    // [ROW];H
                    return new CursorPosition(1, int.Parse(tokens[0].Content.Span, provider: CultureInfo.InvariantCulture));
                }
                else if (IsSequence(tokens, AnsiSequenceTokenType.Integer, AnsiSequenceTokenType.Delimiter))
                {
                    // ;[COLUMN]H
                    return new CursorPosition(int.Parse(tokens[0].Content.Span, provider: CultureInfo.InvariantCulture), 1);
                }
            }
            else if (tokens.Length == 1)
            {
                if (IsSequence(tokens, AnsiSequenceTokenType.Integer))
                {
                    // [ROW]H
                    return new CursorPosition(
                        1,
                        int.Parse(tokens[0].Content.Span, provider: CultureInfo.InvariantCulture));
                }
            }

            return null;
        }

        private static bool IsSequence(ReadOnlySpan<AnsiSequenceToken> tokens, params AnsiSequenceTokenType[] expected)
        {
            if (tokens.Length != expected.Length)
            {
                return false;
            }

            for (var index = 0; index < tokens.Length; index++)
            {
                if (tokens[index].Type != expected[index])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
