using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectre.Terminals
{
    public static partial class ITerminalExtensions
    {
        public static void MoveCursor(this ITerminal terminal, CursorDirection direction, int count)
        {
            if (count <= 0)
            {
                return;
            }

            switch (direction)
            {
                case CursorDirection.Up:
                    terminal.Write($"\u001b[{count}A");
                    break;
                case CursorDirection.Down:
                    terminal.Write($"\u001b[{count}B");
                    break;
                case CursorDirection.Forward:
                    terminal.Write($"\u001b[{count}C");
                    break;
                case CursorDirection.Back:
                    terminal.Write($"\u001b[{count}D");
                    break;
            }
        }

        public static void SetCursorProsition(this ITerminal terminal, int row, int column)
        {
            row = Math.Max(0, row);
            column = Math.Max(0, column);

            terminal.Write($"\u001b[{row};{column}H");
        }

        public static void MoveCursorToNextLine(this ITerminal terminal, int count)
        {
            if (count <= 0)
            {
                return;
            }

            terminal.Write($"\u001b[{count}E");
        }

        public static void MoveCursorToPreviousLine(this ITerminal terminal, int count)
        {
            if (count <= 0)
            {
                return;
            }

            terminal.Write($"\u001b[{count}F");
        }

        public static void MoveCursorToColumn(this ITerminal terminal, int column)
        {
            if (column <= 0)
            {
                return;
            }

            terminal.Write($"\u001b[{column}G");
        }

        public static void Clear(this ITerminal terminal, ClearDisplay option)
        {
            switch (option)
            {
                case ClearDisplay.AfterCursor:
                    terminal.Write($"\u001b[0J");
                    break;
                case ClearDisplay.BeforeCursor:
                    terminal.Write($"\u001b[1J");
                    break;
                case ClearDisplay.Everything:
                    terminal.Write($"\u001b[2J");
                    break;
                case ClearDisplay.EverythingAndScrollbackBuffer:
                    terminal.Write($"\u001b[3J");
                    break;
            }
        }

        public static void Clear(this ITerminal terminal, ClearLine option)
        {
            switch (option)
            {
                case ClearLine.AfterCursor:
                    terminal.Write($"\u001b[0K");
                    break;
                case ClearLine.BeforeCursor:
                    terminal.Write($"\u001b[1K");
                    break;
                case ClearLine.WholeLine:
                    terminal.Write($"\u001b[2K");
                    break;
            }
        }

        public static void SaveCursorPosition(this ITerminal terminal)
        {
            terminal.Write($"\u001b[s");
        }

        public static void RestoreCursorPosition(this ITerminal terminal)
        {
            terminal.Write($"\u001b[u");
        }
    }
}
