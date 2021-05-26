using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spectre.Terminal.Ansi
{
    public static class AnsiSequence
    {
        public static string Clear(string text)
        {
            return AnsiSequenceCleaner.Instance.Run(text);
        }

        public static void Interpret<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context, string text)
        {
            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            var instructions = AnsiInstructionParser.Parse(text.AsMemory());
            foreach (var instruction in instructions)
            {
                instruction.Accept(visitor, context);
            }
        }
    }
}
