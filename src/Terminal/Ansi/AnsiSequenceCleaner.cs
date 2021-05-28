using System;
using System.Text;

namespace Spectre.Terminal.Ansi
{
    internal sealed class AnsiSequenceCleaner : IAnsiSequenceVisitor<StringBuilder>
    {
        internal static AnsiSequenceCleaner Instance { get; } = new AnsiSequenceCleaner();

        public string Run(ReadOnlyMemory<char> buffer)
        {
            var context = new StringBuilder();
            AnsiSequence.Interpret(Instance, context, buffer);
            return context.ToString();
        }

        void IAnsiSequenceVisitor<StringBuilder>.PrintText(PrintText instruction, StringBuilder context)
        {
            context.Append(instruction.Text);
        }

        void IAnsiSequenceVisitor<StringBuilder>.CursorBack(CursorBack instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.CursorDown(CursorDown instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.CursorForward(CursorForward instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.CursorNextLine(CursorNextLine instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.CursorPosition(CursorPosition instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.CursorPreviousLine(CursorPreviousLine instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.CursorUp(CursorUp instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.EraseInDisplay(EraseInDisplay instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.EraseInLine(EraseInLine instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.RestoreCursor(RestoreCursor instruction, StringBuilder context)
        {
        }

        void IAnsiSequenceVisitor<StringBuilder>.SaveCursor(SaveCursor instruction, StringBuilder context)
        {
        }
    }
}
