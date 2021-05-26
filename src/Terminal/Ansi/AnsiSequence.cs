using System;

namespace Spectre.Terminal.Ansi
{
    public static class AnsiSequence
    {
        public static string Clear(string text)
        {
            return AnsiSequenceCleaner.Instance.Run(text.AsMemory());
        }

        public static string Clear(ReadOnlyMemory<char> buffer)
        {
            return AnsiSequenceCleaner.Instance.Run(buffer);
        }

        public static void Interpret<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context, string text)
        {
            Interpret(visitor, context, text.AsMemory());
        }

        public static void Interpret<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context, ReadOnlyMemory<char> buffer)
        {
            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            var instructions = AnsiInstructionParser.Parse(buffer);
            foreach (var instruction in instructions)
            {
                instruction.Accept(visitor, context);
            }
        }
    }
}
