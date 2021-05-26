using System;
using Spectre.Terminal.Ansi;

namespace Spectre.Terminal
{
    internal sealed class ConsoleAnsiInterpreter : AnsiSequenceVisitor<ConsoleAnsiState>
    {
        public static ConsoleAnsiInterpreter Instance { get; } = new ConsoleAnsiInterpreter();

        public void Run(ConsoleAnsiState state, string text)
        {
            AnsiSequence.Interpret(this, state, text);
        }

        protected internal override void EraseInDisplay(EraseInDisplay instruction, ConsoleAnsiState context)
        {
            Console.Clear();
        }

        protected internal override void PrintText(PrintText instruction, ConsoleAnsiState context)
        {
            context.Write(instruction.Text.Span);
        }
    }
}
