// Parts of this code used from: https://github.com/dotnet/runtime
// Licensed to the .NET Foundation under one or more agreements.

using static Spectre.Terminals.Drivers.WindowsConstants;

namespace Spectre.Terminals.Drivers;

internal sealed class WindowsKeyReader
{
    private const short AltVKCode = 0x12;

    private readonly object _lock;
    private readonly SafeHandle _handle;
    private INPUT_RECORD _cachedInputRecord;

    [Flags]
    private enum ControlKeyState
    {
        Unknown = 0,
        RightAltPressed = 0x0001,
        LeftAltPressed = 0x0002,
        RightCtrlPressed = 0x0004,
        LeftCtrlPressed = 0x0008,
        ShiftPressed = 0x0010,
        NumLockOn = 0x0020,
        ScrollLockOn = 0x0040,
        CapsLockOn = 0x0080,
        EnhancedKey = 0x0100,
    }

    public WindowsKeyReader(SafeHandle handle)
    {
        _handle = handle ?? throw new ArgumentNullException(nameof(handle));
        _lock = new object();
    }

    public unsafe bool IsKeyAvailable()
    {
        if (_cachedInputRecord.EventType == KEY_EVENT)
        {
            return true;
        }

        INPUT_RECORD ir;
        var buffer = new Span<INPUT_RECORD>(new INPUT_RECORD[1]);

        while (true)
        {
            var r = PInvoke.PeekConsoleInput(_handle, buffer, out var numEventsRead);
            if (!r)
            {
                throw new InvalidOperationException();
            }

            if (numEventsRead == 0)
            {
                return false;
            }

            ir = buffer[0];

            // Skip non key-down && mod key events.
            if (!IsKeyDownEvent(ir) || IsModKey(ir))
            {
                r = PInvoke.ReadConsoleInput(_handle, buffer, out _);
                if (!r)
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                return true;
            }
        }
    }

    public ConsoleKeyInfo ReadKey()
    {
        INPUT_RECORD ir;
        var buffer = new Span<INPUT_RECORD>(new INPUT_RECORD[1]);

        lock (_lock)
        {
            if (_cachedInputRecord.EventType == KEY_EVENT)
            {
                // We had a previous keystroke with repeated characters.
                ir = _cachedInputRecord;
                if (_cachedInputRecord.Event.KeyEvent.wRepeatCount == 0)
                {
                    _cachedInputRecord.EventType = ushort.MaxValue;
                }
                else
                {
                    _cachedInputRecord.Event.KeyEvent.wRepeatCount--;
                }

                // We will return one key from this method, so we decrement the
                // repeatCount here, leaving the cachedInputRecord in the "queue".
            }
            else
            {
                // We did NOT have a previous keystroke with repeated characters:
                while (true)
                {
                    var r = PInvoke.ReadConsoleInput(_handle, buffer, out var numEventsRead);
                    if (!r || numEventsRead != 1)
                    {
                        // This will fail when stdin is redirected from a file or pipe.
                        // We could theoretically call Console.Read here, but I
                        // think we might do some things incorrectly then.
                        throw new InvalidOperationException("Could not read from STDIN. Has it been redirected?");
                    }

                    ir = buffer[0];
                    var keyCode = ir.Event.KeyEvent.wVirtualKeyCode;

                    // First check for non-keyboard events & discard them. Generally we tap into only KeyDown events and ignore the KeyUp events
                    // but it is possible that we are dealing with a Alt+NumPad unicode key sequence, the final unicode char is revealed only when
                    // the Alt key is released (i.e when the sequence is complete). To avoid noise, when the Alt key is down, we should eat up
                    // any intermediate key strokes (from NumPad) that collectively forms the Unicode character.
                    if (!IsKeyDownEvent(ir))
                    {
                        // REVIEW: Unicode IME input comes through as KeyUp event with no accompanying KeyDown.
                        if (keyCode != AltVKCode)
                        {
                            continue;
                        }
                    }

                    var ch = (char)ir.Event.KeyEvent.uChar.UnicodeChar;

                    // In a Alt+NumPad unicode sequence, when the alt key is released uChar will represent the final unicode character, we need to
                    // surface this. VirtualKeyCode for this event will be Alt from the Alt-Up key event. This is probably not the right code,
                    // especially when we don't expose ConsoleKey.Alt, so this will end up being the hex value (0x12). VK_PACKET comes very
                    // close to being useful and something that we could look into using for this purpose...
                    if (ch == 0)
                    {
                        // Skip mod keys.
                        if (IsModKey(ir))
                        {
                            continue;
                        }
                    }

                    // When Alt is down, it is possible that we are in the middle of a Alt+NumPad unicode sequence.
                    // Escape any intermediate NumPad keys whether NumLock is on or not (notepad behavior)
                    var key = (ConsoleKey)keyCode;
                    if (IsAltKeyDown(ir) && ((key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9)
                                         || (key == ConsoleKey.Clear) || (key == ConsoleKey.Insert)
                                         || (key >= ConsoleKey.PageUp && key <= ConsoleKey.DownArrow)))
                    {
                        continue;
                    }

                    if (ir.Event.KeyEvent.wRepeatCount > 1)
                    {
                        ir.Event.KeyEvent.wRepeatCount--;
                        _cachedInputRecord = ir;
                    }

                    break;
                }
            }

            // we did NOT have a previous keystroke with repeated characters.
        }

        var state = (ControlKeyState)ir.Event.KeyEvent.dwControlKeyState;
        var shift = (state & ControlKeyState.ShiftPressed) != 0;
        var alt = (state & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;
        var control = (state & (ControlKeyState.LeftCtrlPressed | ControlKeyState.RightCtrlPressed)) != 0;

        return new ConsoleKeyInfo(
            (char)ir.Event.KeyEvent.uChar.UnicodeChar,
            (ConsoleKey)ir.Event.KeyEvent.wVirtualKeyCode,
            shift, alt, control);
    }

    private static bool IsKeyDownEvent(INPUT_RECORD ir)
    {
        return ir.EventType == KEY_EVENT && ir.Event.KeyEvent.bKeyDown.Value != 0;
    }

    private static bool IsModKey(INPUT_RECORD ir)
    {
        // We should also skip over Shift, Control, and Alt, as well as caps lock.
        // Apparently we don't need to check for 0xA0 through 0xA5, which are keys like
        // Left Control & Right Control. See the ConsoleKey enum for these values.
        var keyCode = ir.Event.KeyEvent.wVirtualKeyCode;
        return (keyCode >= 0x10 && keyCode <= 0x12) || keyCode == 0x14 || keyCode == 0x90 || keyCode == 0x91;
    }

    // For tracking Alt+NumPad unicode key sequence. When you press Alt key down
    // and press a numpad unicode decimal sequence and then release Alt key, the
    // desired effect is to translate the sequence into one Unicode KeyPress.
    // We need to keep track of the Alt+NumPad sequence and surface the final
    // unicode char alone when the Alt key is released.
    private static bool IsAltKeyDown(INPUT_RECORD ir)
    {
        return (((ControlKeyState)ir.Event.KeyEvent.dwControlKeyState)
            & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;
    }
}