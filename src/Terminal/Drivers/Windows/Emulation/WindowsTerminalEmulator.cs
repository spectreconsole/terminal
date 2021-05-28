using System;
using Spectre.Terminal.Ansi;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminal
{
    internal sealed class WindowsTerminalEmulator : IAnsiSequenceVisitor<WindowsTerminalState>
    {
        public void Write(WindowsTerminalState state, ReadOnlySpan<byte> buffer)
        {
            // TODO: Not very efficient
            var text = state.Writer.Encoding.GetString(buffer);
            AnsiSequence.Interpret(this, state, text);
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorUp(CursorUp op, WindowsTerminalState state)
        {
            MoveCursorRelative(state, y: -1);
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorDown(CursorDown op, WindowsTerminalState state)
        {
            MoveCursorRelative(state, y: 1);
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorBack(CursorBack op, WindowsTerminalState state)
        {
            MoveCursorRelative(state, x: -1);
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorForward(CursorForward op, WindowsTerminalState state)
        {
            MoveCursorRelative(state, x: 1);
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorHorizontalAbsolute(CursorHorizontalAbsolute op, WindowsTerminalState state)
        {
            if (PInvoke.GetConsoleScreenBufferInfo(state.Handle, out var info))
            {
                info.dwCursorPosition.X = (short)op.Column;
                SetCursorPosition(state, info.dwCursorPosition);
            }
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorNextLine(CursorNextLine op, WindowsTerminalState state)
        {
            if (PInvoke.GetConsoleScreenBufferInfo(state.Handle, out var info))
            {
                info.dwCursorPosition.X = 0;
                info.dwCursorPosition.Y += (short)(1 * op.Count);
                SetCursorPosition(state, info.dwCursorPosition);
            }
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorPreviousLine(CursorPreviousLine op, WindowsTerminalState state)
        {
            if (PInvoke.GetConsoleScreenBufferInfo(state.Handle, out var info))
            {
                info.dwCursorPosition.X = 0;
                info.dwCursorPosition.Y -= (short)(1 * op.Count);
                SetCursorPosition(state, info.dwCursorPosition);
            }
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.CursorPosition(CursorPosition op, WindowsTerminalState state)
        {
            SetCursorPosition(state, new COORD
            {
                X = (short)op.Column,
                Y = (short)op.Row,
            });
        }

        unsafe void IAnsiSequenceVisitor<WindowsTerminalState>.EraseInDisplay(EraseInDisplay op, WindowsTerminalState state)
        {
            if (!PInvoke.GetConsoleScreenBufferInfo(state.Handle, out var info))
            {
                return;
            }

            if (op.Mode == 2 || op.Mode == 3)
            {
                var terminalSize = info.dwSize.X * info.dwSize.Y;
                if (PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)terminalSize, new COORD(), out _))
                {
                    // Set the cursor to home
                    SetCursorPosition(state, new COORD
                    {
                        X = 0,
                        Y = 0,
                    });
                }
            }
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.EraseInLine(EraseInLine op, WindowsTerminalState state)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.PrintText(PrintText op, WindowsTerminalState state)
        {
            // TODO: Not very efficient
            var bytes = state.Writer.Encoding.GetBytes(op.Text.ToArray());
            state.Writer.Write(new ReadOnlySpan<byte>(bytes));
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.RestoreCursor(RestoreCursor op, WindowsTerminalState state)
        {
            if (state.StoredCursorPosition != null)
            {
                SetCursorPosition(state, state.StoredCursorPosition.Value);
            }
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.SaveCursor(SaveCursor op, WindowsTerminalState state)
        {
            if (PInvoke.GetConsoleScreenBufferInfo(state.Handle, out var info))
            {
                state.StoredCursorPosition = info.dwCursorPosition;
            }
            else
            {
                state.StoredCursorPosition = null;
            }
        }

        private static void MoveCursorRelative(WindowsTerminalState state, short x = 0, short y = 0)
        {
            if (PInvoke.GetConsoleScreenBufferInfo(state.Handle, out var info))
            {
                info.dwCursorPosition.X += x;
                info.dwCursorPosition.Y += y;
                SetCursorPosition(state, info.dwCursorPosition);
            }
        }

        private static void SetCursorPosition(WindowsTerminalState state, COORD coordinates)
        {
            PInvoke.SetConsoleCursorPosition(state.Handle, coordinates);
        }
    }
}
