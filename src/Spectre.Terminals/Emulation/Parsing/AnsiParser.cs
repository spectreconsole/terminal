namespace Spectre.Terminals.Emulation;

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
            'm' => ParseSgr(parameters),
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

        return func(ParseInteger(tokens[0].Content.Span));
    }

    private static int ParseInteger(ReadOnlySpan<char> span, IFormatProvider? provider = null)
    {
#if NET5_0_OR_GREATER
        return int.Parse(span, provider: provider);
#else
        return int.Parse(new string(span.ToArray()), provider);
#endif
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

        if (predicate(ParseInteger(tokens[0].Content.Span)))
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
                    ParseInteger(tokens[2].Content.Span, CultureInfo.InvariantCulture),
                    ParseInteger(tokens[0].Content.Span, CultureInfo.InvariantCulture));
            }
        }
        else if (tokens.Length == 2)
        {
            if (IsSequence(tokens, AnsiSequenceTokenType.Integer, AnsiSequenceTokenType.Delimiter))
            {
                // [ROW];H
                return new CursorPosition(1, ParseInteger(tokens[0].Content.Span, CultureInfo.InvariantCulture));
            }
            else if (IsSequence(tokens, AnsiSequenceTokenType.Integer, AnsiSequenceTokenType.Delimiter))
            {
                // ;[COLUMN]H
                return new CursorPosition(ParseInteger(tokens[0].Content.Span, CultureInfo.InvariantCulture), 1);
            }
        }
        else if (tokens.Length == 1)
        {
            if (IsSequence(tokens, AnsiSequenceTokenType.Integer))
            {
                // [ROW]H
                return new CursorPosition(
                    1,
                    ParseInteger(tokens[0].Content.Span, CultureInfo.InvariantCulture));
            }
        }

        return null;
    }

    private static SelectGraphicRendition? ParseSgr(ReadOnlySpan<AnsiSequenceToken> tokens)
    {
        if (tokens.Length == 0)
        {
            // No parameters is treated like a reset
            return new SelectGraphicRendition(new[]
            {
                new SelectGraphicRendition.Operation
                {
                    Reset = true,
                },
            });
        }

        var queue = new Queue<int>();
        var enumerator = tokens.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Type == AnsiSequenceTokenType.Integer)
            {
                queue.Enqueue(ParseInteger(enumerator.Current.Content.Span));
            }
        }

        var ops = new List<SelectGraphicRendition.Operation>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == 0)
            {
                // Reset
                ops.Add(new SelectGraphicRendition.Operation
                {
                    Reset = true,
                });
            }
            else if (current >= 30 && current <= 37)
            {
                // 3-bit Foreground number
                ops.Add(new SelectGraphicRendition.Operation
                {
                    Foreground = new Color(current - 30),
                });
            }
            else if (current >= 40 && current <= 47)
            {
                // 3-bit background number
                ops.Add(new SelectGraphicRendition.Operation
                {
                    Background = new Color(current - 40),
                });
            }
            else if (current == 38 || current == 48)
            {
                // 3, 4, or 8-bit colors require at least two arguments
                if (queue.Count < 2)
                {
                    // Invalid
                    return null;
                }

                var isForeground = current == 38;

                current = queue.Dequeue();
                if (current == 5)
                {
                    // Color number
                    if (queue.Count == 0)
                    {
                        // Invalid
                        return null;
                    }

                    if (isForeground)
                    {
                        ops.Add(new SelectGraphicRendition.Operation
                        {
                            Foreground = new Color(queue.Dequeue()),
                        });
                    }
                    else
                    {
                        ops.Add(new SelectGraphicRendition.Operation
                        {
                            Background = new Color(queue.Dequeue()),
                        });
                    }
                }
                else if (current == 2)
                {
                    // 24-bit colors requires at least three arguments
                    if (queue.Count < 3)
                    {
                        // Invalid
                        return null;
                    }

                    if (isForeground)
                    {
                        ops.Add(new SelectGraphicRendition.Operation
                        {
                            Foreground = new Color(queue.Dequeue(), queue.Dequeue(), queue.Dequeue()),
                        });
                    }
                    else
                    {
                        ops.Add(new SelectGraphicRendition.Operation
                        {
                            Background = new Color(queue.Dequeue(), queue.Dequeue(), queue.Dequeue()),
                        });
                    }
                }
            }
        }

        return new SelectGraphicRendition(ops);
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
