using System;
using System.Runtime.InteropServices;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminals.Windows.Emulation
{
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
                        (int)((buffer.ColorTable[index] >> 16) & 0xff),
                        (int)((buffer.ColorTable[index] >> 8) & 0xff),
                        (int)(buffer.ColorTable[index] & 0xff));
                }

                // Get the default colors
                _defaultColors = (byte)(buffer.wAttributes & WindowsConstants.Colors.ColorMask);
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

            if (color.IsRgb)
            {
                // TODO: Find the closest color in the default palette.
            }

            if (number != null && number.Value >= 0 && number.Value < 16)
            {
                var c = GetColorAttribute(number.Value, !foreground);
                if (c == null)
                {
                    return;
                }

                if (TryGetConsoleBuffer(out var buffer))
                {
                    var attrs = (short)buffer.wAttributes;
                    if (foreground)
                    {
                        attrs &= ~WindowsConstants.Colors.ForegroundMask;
                    }
                    else
                    {
                        attrs &= ~WindowsConstants.Colors.BackgroundMask;
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

            var result = color;
            if (isBackground)
            {
                result <<= 4;
            }

            return result;
        }
    }
}
