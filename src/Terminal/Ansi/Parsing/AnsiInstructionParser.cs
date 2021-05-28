using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Spectre.Terminal.Ansi
{
    internal sealed class AnsiInstructionParser
    {
        public static IEnumerable<AnsiInstruction> Parse(ReadOnlyMemory<char> buffer)
        {
            foreach (var token in AnsiInstructionTokenizer.Tokenize(buffer))
            {
                if (token.IsText)
                {
                    yield return new PrintText(token.Span);
                }
                else
                {
                    var instruction = ParseInstruction(token.Tokens.ToArray());
                    if (instruction != null)
                    {
                        yield return instruction;
                    }
                }
            }
        }

        private static AnsiInstruction? ParseInstruction(AnsiSequenceToken[] tokens)
        {
            if (tokens.Length == 0 || tokens[0].Type != AnsiSequenceTokenType.Csi)
            {
                return null;
            }

            var terminal = tokens[tokens.Length - 1].AsCharacter();
            if (terminal == null)
            {
                return null;
            }

            // Get the parameters
            var parameters = new Span<AnsiSequenceToken>(tokens)[1..^1];

            // Create the instruction
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
                's' => new SaveCursor(),
                'u' => new RestoreCursor(),
                _ => null, // Unknown instruction
            };
        }

        private static AnsiInstruction? ParseIntegerInstruction(ReadOnlySpan<AnsiSequenceToken> tokens, Func<int, AnsiInstruction> func, int defaultValue = 1)
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

        private static CursorPosition? ParseCursorPosition(ReadOnlySpan<AnsiSequenceToken> tokens)
        {
            if (tokens.Length == 3)
            {
                if (IsSequence(tokens, AnsiSequenceTokenType.Integer, AnsiSequenceTokenType.Delimiter, AnsiSequenceTokenType.Integer))
                {
                    // X;YH
                    return new CursorPosition(
                        int.Parse(tokens[0].Content.Span, provider: CultureInfo.InvariantCulture),
                        int.Parse(tokens[2].Content.Span, provider: CultureInfo.InvariantCulture));
                }
            }
            else if (tokens.Length == 2)
            {
                if (IsSequence(tokens, AnsiSequenceTokenType.Integer, AnsiSequenceTokenType.Delimiter))
                {
                    // X;H
                    return new CursorPosition(int.Parse(tokens[0].Content.Span, provider: CultureInfo.InvariantCulture), 1);
                }
                else if (IsSequence(tokens, AnsiSequenceTokenType.Integer, AnsiSequenceTokenType.Delimiter))
                {
                    // ;YH
                    return new CursorPosition(1, int.Parse(tokens[0].Content.Span, provider: CultureInfo.InvariantCulture));
                }
            }
            else if (tokens.Length == 1)
            {
                if (IsSequence(tokens, AnsiSequenceTokenType.Integer))
                {
                    // XH
                    return new CursorPosition(
                        int.Parse(tokens[0].Content.Span, provider: CultureInfo.InvariantCulture),
                        1);
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
