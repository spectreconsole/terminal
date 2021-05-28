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
            var text = state.Encoding.GetString(buffer);
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

            if (op.Mode == 0)
            {
                // Delete everything after the cursor
                var skip = ((info.dwCursorPosition.Y - 1) * info.dwSize.X) + info.dwCursorPosition.X;
                var take = (info.dwSize.X * info.dwSize.Y) - skip;
                PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)take, new COORD()
                {
                    X = info.dwCursorPosition.X,
                    Y = info.dwCursorPosition.Y,
                }, out _);
            }
            else if (op.Mode == 1)
            {
                // Delete everything before the cursor
                var skip = (info.dwCursorPosition.Y * info.dwSize.X) + info.dwCursorPosition.X;
                PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)skip, new COORD()
                {
                    X = 0,
                    Y = 0,
                }, out _);
            }
            else if (op.Mode == 2)
            {
                // Delete everything
                var terminalSize = info.dwSize.X * info.dwSize.Y;
                PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)terminalSize, new COORD(), out _);
            }
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.EraseInLine(EraseInLine op, WindowsTerminalState state)
        {
        }

        void IAnsiSequenceVisitor<WindowsTerminalState>.PrintText(PrintText op, WindowsTerminalState state)
        {
            // TODO: Not very efficient
            var bytes = state.Encoding.GetBytes(op.Text.ToArray());
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
            coordinates.X = (short)Math.Max(coordinates.X - 1, 0);
            coordinates.Y = (short)Math.Max(coordinates.Y - 1, 0);

            PInvoke.SetConsoleCursorPosition(state.Handle, coordinates);
        }
    }
}
