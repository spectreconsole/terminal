using System;
using System.Collections.Generic;

namespace Spectre.Terminals.Emulation
{
    internal static class AnsiSequenceSplitter
    {
        public static List<(ReadOnlyMemory<char> Text, bool IsSequence)> Split(ReadOnlyMemory<char> buffer)
        {
            var index = 0;
            var end = 0;

            var result = new List<(ReadOnlyMemory<char>, bool)>();
            while (index < buffer.Length)
            {
                // Encounter ESC?
                if (buffer.Span[index] == 0x1b)
                {
                    var start = index;
                    index++;

                    if (index > buffer.Length - 1)
                    {
                        break;
                    }

                    // Not CSI?
                    if (buffer.Span[index] != '[')
                    {
                        continue;
                    }

                    index++;

                    if (index >= buffer.Length)
                    {
                        break;
                    }

                    // Any number (including none) of "parameter bytes" in the range 0x30–0x3F
                    if (!EatOptionalRange(buffer, 0x30, 0x3f, ref index))
                    {
                        break;
                    }

                    // Any number of "intermediate bytes" in the range 0x20–0x2F
                    if (!EatOptionalRange(buffer, 0x20, 0x2f, ref index))
                    {
                        break;
                    }

                    // A single "final byte" in the range 0x40–0x7E
                    var terminal = buffer.Span[index];
                    if (terminal < 0x40 || terminal > 0x7e)
                    {
                        throw new InvalidOperationException("Malformed ANSI escape code");
                    }

                    index++;

                    // Need to flush?
                    if (end < start)
                    {
                        result.Add((buffer[end..start], false));
                    }

                    // Add the escape code to the result
                    end = index;
                    result.Add((buffer[start..end], true));

                    continue;
                }

                index++;
            }

            // More to flush?
            if (end < buffer.Length)
            {
                result.Add((buffer[end..buffer.Length], false));
            }

            return result;
        }

        private static bool EatOptionalRange(ReadOnlyMemory<char> buffer, int start, int stop, ref int index)
        {
            while (true)
            {
                if (index >= buffer.Length)
                {
                    return false;
                }

                var current1 = buffer.Span[index];
                if (current1 < start || current1 > stop)
                {
                    return true;
                }

                index++;
            }
        }
    }
}
