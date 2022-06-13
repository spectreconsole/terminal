namespace Spectre.Terminals.Drivers;

internal sealed class WindowsColors
{
    private readonly SafeHandle _stdout;
    private readonly SafeHandle _stderr;
    private readonly Color[]? _colorTable;
    private readonly short? _defaultColors;

    public WindowsColors(SafeHandle stdout, SafeHandle stderr)
    {
        _stdout = stdout ?? throw new ArgumentNullException(nameof(stdout));
        _stderr = stderr ?? throw new ArgumentNullException(nameof(stderr));

        if (TryGetConsoleBuffer(out var buffer))
        {
            // Build the color table
            // We will use this to find the nearest color.
            _colorTable = new Color[16];
            for (var index = 0; index < 16; index++)
            {
                _colorTable[index] = new Color(
                    (int)((GetValue(ref buffer.ColorTable, index) >> 16) & 0xff),
                    (int)((GetValue(ref buffer.ColorTable, index) >> 8) & 0xff),
                    (int)(GetValue(ref buffer.ColorTable, index) & 0xff));
            }

            // Get the default colors
            _defaultColors = (byte)(buffer.wAttributes & WindowsConstants.Colors.COLOR_MASK);
        }
    }

    public void SetForeground(Color color)
    {
        SetColor(color, foreground: true);
    }

    public void SetBackground(Color color)
    {
        SetColor(color, foreground: false);
    }

    public void Reset()
    {
        if (_defaultColors != null)
        {
            PInvoke.SetConsoleTextAttribute(_stdout, (ushort)_defaultColors.Value);
        }
    }

    private uint GetValue(ref CONSOLE_SCREEN_BUFFER_INFOEX.__uint_16 value, int index)
    {
#if NET5_0_OR_GREATER
        return value[index];
#else
        return index switch
        {
            0 => value._0,
            1 => value._1,
            2 => value._2,
            3 => value._3,
            4 => value._4,
            5 => value._5,
            6 => value._6,
            7 => value._7,
            8 => value._8,
            9 => value._9,
            10 => value._10,
            11 => value._11,
            12 => value._12,
            13 => value._13,
            14 => value._14,
            15 => value._15,
            _ => throw new InvalidOperationException("Invalid __uint_16 index"),
        };
#endif
    }

    private unsafe bool TryGetConsoleBuffer(out CONSOLE_SCREEN_BUFFER_INFOEX info)
    {
        info = default(CONSOLE_SCREEN_BUFFER_INFOEX);
        info.cbSize = (uint)sizeof(CONSOLE_SCREEN_BUFFER_INFOEX);

        if (PInvoke.GetConsoleScreenBufferInfoEx(_stdout, ref info) ||
            PInvoke.GetConsoleScreenBufferInfoEx(_stderr, ref info))
        {
            return true;
        }

        return false;
    }

    private void SetColor(Color color, bool foreground)
    {
        var number = color.Number;

        if (color.IsRgb && number == null)
        {
            // TODO: Find the closest color
            return;
        }

        if (number != null)
        {
            var colorNumber = number.Value;
            if (colorNumber >= 0 && colorNumber < 16)
            {
                // Map the number to 4-bit color
                colorNumber = Map4BitColor(colorNumber);
            }
            else if (colorNumber < 256)
            {
                // TODO: Support 256-bit colors
                return;
            }

            var c = GetColorAttribute(colorNumber, !foreground);
            if (c == null)
            {
                return;
            }

            if (TryGetConsoleBuffer(out var buffer))
            {
                var attrs = (short)buffer.wAttributes;
                if (foreground)
                {
                    attrs &= ~WindowsConstants.Colors.FOREGROUND_MASK;
                }
                else
                {
                    attrs &= ~WindowsConstants.Colors.BACKGROUND_MASK;
                }

                attrs = (short)(((uint)(ushort)attrs) | (ushort)c.Value);
                PInvoke.SetConsoleTextAttribute(_stdout, (ushort)attrs);
            }
        }
    }

    private static int? GetColorAttribute(int color, bool isBackground)
    {
        if ((color & ~0xf) != 0)
        {
            return null;
        }

        if (isBackground)
        {
            color <<= 4;
        }

        return color;
    }

    private static int Map4BitColor(int number)
    {
        return number switch
        {
            0 => 0,
            1 => 4,
            2 => 2,
            3 => 6,
            4 => 1,
            5 => 5,
            6 => 3,
            7 => 7,
            8 => 8,
            9 => 12,
            10 => 10,
            11 => 14,
            12 => 9,
            13 => 13,
            14 => 11,
            15 => 15,
            _ => number,
        };
    }
}
