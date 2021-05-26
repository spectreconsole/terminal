using System;
using System.Text;

namespace Spectre.Terminal.Ansi
{
    internal sealed class AnsiSequenceCleaner : AnsiSequenceVisitor<StringBuilder>
    {
        internal static AnsiSequenceCleaner Instance { get; } = new AnsiSequenceCleaner();

        public string Run(ReadOnlyMemory<char> buffer)
        {
            var context = new StringBuilder();
            AnsiSequence.Interpret(Instance, context, buffer);
            return context.ToString();
        }

        protected internal override void PrintText(PrintText instruction, StringBuilder context)
        {
            context.Append(instruction.Text);
        }
    }
}
