using System;

namespace Spectre.Terminals
{
    public static partial class ITerminalExtensions
    {
        public static byte? ReadRaw(this ITerminal terminal)
        {
            try
            {
                terminal.EnableRawMode();
                Span<byte> span = stackalloc byte[1];
                return terminal.Input.Read(span) == 1 ? span[0] : null;
            }
            finally
            {
                terminal.DisableRawMode();
            }
        }

        public static void Write(this ITerminal terminal, ReadOnlySpan<char> value)
        {
            terminal.Output.Write(value);
        }

        public static void Write(this ITerminal terminal, string? value)
        {
            terminal.Output.Write(value);
        }

        public static void WriteLine(this ITerminal terminal)
        {
            terminal.Output.WriteLine();
        }

        public static void WriteLine(this ITerminal terminal, string? value)
        {
            terminal.Output.WriteLine(value);
        }


    }
}
