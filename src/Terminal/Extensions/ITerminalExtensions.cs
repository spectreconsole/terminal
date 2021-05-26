using System;

namespace Spectre.Terminal
{
    public static class ITerminalExtensions
    {
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
