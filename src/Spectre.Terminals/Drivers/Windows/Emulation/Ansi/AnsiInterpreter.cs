using System;

namespace Spectre.Terminals.Windows.Emulation
{
    internal static class AnsiInterpreter
    {
        public static void Interpret<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context, string text)
        {
            Interpret(visitor, context, text.AsMemory());
        }

        public static void Interpret<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context, ReadOnlyMemory<char> buffer)
        {
            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            var instructions = AnsiParser.Parse(buffer);
            foreach (var instruction in instructions)
            {
                instruction.Accept(visitor, context);
            }
        }
    }
}
