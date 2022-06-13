namespace Spectre.Terminals;

/// <summary>
/// Contains extension methods for <see cref="ITerminal"/>.
/// </summary>
public static partial class ITerminalExtensions
{
    /// <summary>
    /// Moves the cursor.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="direction">The direction to move the cursor.</param>
    /// <param name="count">The number of steps to move the cursor.</param>
    public static void MoveCursor(this ITerminal terminal, CursorDirection direction, int count = 1)
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

    /// <summary>
    /// Sets the cursor position.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
    public static void SetCursorProsition(this ITerminal terminal, int row, int column)
    {
        row = Math.Max(0, row);
        column = Math.Max(0, column);

        terminal.Write($"\u001b[{row};{column}H");
    }

    /// <summary>
    /// Moves the cursor down and resets the column position.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="count">The number of lines to move down.</param>
    public static void MoveCursorToNextLine(this ITerminal terminal, int count)
    {
        if (count <= 0)
        {
            return;
        }

        terminal.Write($"\u001b[{count}E");
    }

    /// <summary>
    /// Moves the cursor up and resets the column position.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="count">The number of lines to move up.</param>
    public static void MoveCursorToPreviousLine(this ITerminal terminal, int count)
    {
        if (count <= 0)
        {
            return;
        }

        terminal.Write($"\u001b[{count}F");
    }

    /// <summary>
    /// Moves the cursor to the specified column.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="column">The column to move to.</param>
    public static void MoveCursorToColumn(this ITerminal terminal, int column)
    {
        if (column <= 0)
        {
            return;
        }

        terminal.Write($"\u001b[{column}G");
    }

    /// <summary>
    /// Clears the display.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="option">The clear options.</param>
    public static void Clear(this ITerminal terminal, ClearDisplay option = ClearDisplay.Everything)
    {
        switch (option)
        {
            case ClearDisplay.AfterCursor:
                terminal.Write("\u001b[0J");
                break;
            case ClearDisplay.BeforeCursor:
                terminal.Write("\u001b[1J");
                break;
            case ClearDisplay.Everything:
                terminal.Write("\u001b[2J");
                break;
            case ClearDisplay.EverythingAndScrollbackBuffer:
                terminal.Write("\u001b[3J");
                break;
        }
    }

    /// <summary>
    /// Clears the current line.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    /// <param name="option">The clear options.</param>
    public static void Clear(this ITerminal terminal, ClearLine option)
    {
        switch (option)
        {
            case ClearLine.AfterCursor:
                terminal.Write("\u001b[0K");
                break;
            case ClearLine.BeforeCursor:
                terminal.Write("\u001b[1K");
                break;
            case ClearLine.WholeLine:
                terminal.Write("\u001b[2K");
                break;
        }
    }

    /// <summary>
    /// Saves the cursor position.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    public static void SaveCursorPosition(this ITerminal terminal)
    {
        terminal.Write("\u001b[s");
    }

    /// <summary>
    /// Restores the cursor position.
    /// </summary>
    /// <param name="terminal">The terminal.</param>
    public static void RestoreCursorPosition(this ITerminal terminal)
    {
        terminal.Write("\u001b[u");
    }
}
