namespace Spectre.Terminals.Drivers;

internal sealed class WindowsTerminalEmulator : IAnsiSequenceVisitor<WindowsTerminalState>
{
    public void Write(WindowsTerminalState state, ReadOnlySpan<byte> buffer)
    {
        // TODO: Not very efficient
#if NET5_0_OR_GREATER
        var text = state.Encoding.GetString(buffer);
#else
        var text = state.Encoding.GetString(buffer.ToArray());
#endif
        AnsiInterpreter.Interpret(this, state, text);
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
            var length = (info.dwSize.X * info.dwSize.Y) - skip;
            PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)length, new COORD()
            {
                X = info.dwCursorPosition.X,
                Y = info.dwCursorPosition.Y,
            }, out _);
        }
        else if (op.Mode == 1)
        {
            // Delete everything before the cursor
            var length = (info.dwCursorPosition.Y * info.dwSize.X) + info.dwCursorPosition.X;
            PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)length, default(COORD), out _);
        }
        else if (op.Mode == 2 || op.Mode == 3)
        {
            // Delete everything
            var terminalSize = info.dwSize.X * info.dwSize.Y;
            PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)terminalSize, default(COORD), out _);
        }
    }

    void IAnsiSequenceVisitor<WindowsTerminalState>.EraseInLine(EraseInLine op, WindowsTerminalState state)
    {
        if (!PInvoke.GetConsoleScreenBufferInfo(state.Handle, out var info))
        {
            return;
        }

        if (op.Mode == 0)
        {
            // Delete line after the cursor
            var length = info.dwSize.X - info.dwCursorPosition.X;
            PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)length, info.dwCursorPosition, out _);
        }
        else if (op.Mode == 1)
        {
            // Delete line before the cursor
            var length = info.dwCursorPosition.X;
            var pos = new COORD { X = 0, Y = info.dwCursorPosition.Y };
            PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)length, pos, out _);
        }
        else if (op.Mode == 2)
        {
            // Delete whole line
            var pos = new COORD { X = 0, Y = info.dwCursorPosition.Y };
            PInvoke.FillConsoleOutputCharacter(state.Handle, ' ', (uint)info.dwSize.X, pos, out _);
        }
    }

    void IAnsiSequenceVisitor<WindowsTerminalState>.PrintText(PrintText op, WindowsTerminalState state)
    {
        // TODO: Not very efficient
        var bytes = state.Encoding.GetBytes(op.Text.ToArray());
        state.Writer.Write(state.Handle, new ReadOnlySpan<byte>(bytes));
    }

    void IAnsiSequenceVisitor<WindowsTerminalState>.RestoreCursor(RestoreCursor op, WindowsTerminalState state)
    {
        if (state.StoredCursorPosition != null)
        {
            SetCursorPosition(state, state.StoredCursorPosition.Value);
        }
    }

    void IAnsiSequenceVisitor<WindowsTerminalState>.StoreCursor(StoreCursor op, WindowsTerminalState state)
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

    void IAnsiSequenceVisitor<WindowsTerminalState>.HideCursor(HideCursor instruction, WindowsTerminalState state)
    {
        if (PInvoke.GetConsoleCursorInfo(state.Handle, out var info))
        {
            PInvoke.SetConsoleCursorInfo(state.Handle, new CONSOLE_CURSOR_INFO
            {
                bVisible = false,
                dwSize = info.dwSize,
            });
        }
    }

    void IAnsiSequenceVisitor<WindowsTerminalState>.ShowCursor(ShowCursor instruction, WindowsTerminalState state)
    {
        if (PInvoke.GetConsoleCursorInfo(state.Handle, out var info))
        {
            PInvoke.SetConsoleCursorInfo(state.Handle, new CONSOLE_CURSOR_INFO
            {
                bVisible = true,
                dwSize = info.dwSize,
            });
        }
    }

    unsafe void IAnsiSequenceVisitor<WindowsTerminalState>.EnableAlternativeBuffer(
        EnableAlternativeBuffer instruction, WindowsTerminalState state)
    {
        if (state.AlternativeBuffer != null)
        {
            return;
        }

        // Try creating the screen buffer
        state.AlternativeBuffer = PInvoke.CreateConsoleScreenBuffer(
            WindowsConstants.GENERIC_WRITE, WindowsConstants.FILE_SHARE_WRITE,
            (SECURITY_ATTRIBUTES?)null, WindowsConstants.CONSOLE_TEXTMODE_BUFFER, null);

        // Set the screenbuffer if successful
        if (state.AlternativeBuffer != null)
        {
            PInvoke.SetConsoleActiveScreenBuffer(state.AlternativeBuffer);
            SetCursorPosition(state, new COORD
            {
                X = 0,
                Y = 0,
            });
        }
    }

    void IAnsiSequenceVisitor<WindowsTerminalState>.DisableAlternativeBuffer(
        DisableAlternativeBuffer instruction, WindowsTerminalState state)
    {
        if (state.AlternativeBuffer != null)
        {
            if (PInvoke.SetConsoleActiveScreenBuffer(state.MainBuffer))
            {
                var handle = state.AlternativeBuffer.DangerousGetHandle();
                if (handle != IntPtr.Zero)
                {
                    PInvoke.CloseHandle(new HANDLE(handle));
                    state.AlternativeBuffer = null;
                }
            }
        }
    }

    void IAnsiSequenceVisitor<WindowsTerminalState>.SelectGraphicRendition(
        SelectGraphicRendition instruction, WindowsTerminalState state)
    {
        foreach (var operation in instruction.Operations)
        {
            if (operation.Reset)
            {
                state.Colors.Reset();
            }
            else if (operation.Foreground != null)
            {
                state.Colors.SetForeground(operation.Foreground.Value);
            }
            else if (operation.Background != null)
            {
                state.Colors.SetBackground(operation.Background.Value);
            }
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
