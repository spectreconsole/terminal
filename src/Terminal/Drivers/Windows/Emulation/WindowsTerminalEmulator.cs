using System;
using System.Text;
using Spectre.Terminal.Ansi;

namespace Spectre.Terminal
{
    internal sealed class WindowsTerminalEmulator : IAnsiSequenceVisitor<WindowsTerminalState>
    {
        public void Write(WindowsTerminalState state, ReadOnlySpan<byte> buffer)
        {
            AnsiSequence.Interpret(this, state, buffer.ToString());
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorBack(CursorBack instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorDown(CursorDown instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorForward(CursorForward instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorNextLine(CursorNextLine instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorPosition(CursorPosition instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorPreviousLine(CursorPreviousLine instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorUp(CursorUp instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.EraseInDisplay(EraseInDisplay instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.EraseInLine(EraseInLine instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.PrintText(PrintText instruction, WindowsTerminalState context)
        {
            // TODO: Not very efficient
            var bytes = context.Writer.Encoding.GetBytes(instruction.Text.ToArray());
            context.Writer.Write(new ReadOnlySpan<byte>(bytes));
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.RestoreCursor(RestoreCursor instruction, WindowsTerminalState context)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.SaveCursor(SaveCursor instruction, WindowsTerminalState context)
        {
        }
    }
}
