using System.Text;

namespace Spectre.Terminal.Ansi
{
    internal sealed class AnsiSequenceCleaner : AnsiSequenceVisitor<StringBuilder>
    {
        internal static AnsiSequenceCleaner Instance { get; } = new AnsiSequenceCleaner();

        public string Run(string text)
        {
            var context = new StringBuilder();
            AnsiSequence.Interpret(Instance, context, text);
            return context.ToString();
        }

        protected internal override void PrintText(PrintText instruction, StringBuilder context)
        {
            context.Append(instruction.Text);
        }
    }
}
