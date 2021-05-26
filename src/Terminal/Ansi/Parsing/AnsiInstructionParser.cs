using System;
using System.Collections.Generic;
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

            // Get the terminal
            var terminal = tokens[tokens.Length - 1].AsCharacter();
            if (terminal == null)
            {
                return null;
            }

            // Get the parameters minus the terminal
            var parameters = new Span<AnsiSequenceToken>(tokens)[1..^1];

            // Create the instruction
            return terminal.Value switch
            {
                'A' => ParseIntegerInstruction(parameters, count => new CursorUp(count)),
                'B' => ParseIntegerInstruction(parameters, count => new CursorDown(count)),
                'C' => ParseIntegerInstruction(parameters, count => new CursorForward(count)),
                'D' => ParseIntegerInstruction(parameters, count => new CursorBack(count)),
                'E' => ParseIntegerInstruction(parameters, count => new CursorNextLine(count)),
                'F' => ParseIntegerInstruction(parameters, count => new CursorPreviousLine(count)),
                'G' => ParseIntegerInstruction(parameters, count => new CursorHorizontalAbsolute(count)),
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
    }
}
