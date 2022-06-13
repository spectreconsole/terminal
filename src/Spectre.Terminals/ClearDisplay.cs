namespace Spectre.Terminals;

/// <summary>
/// Represents different ways of clearing a display.
/// </summary>
public enum ClearDisplay
{
    /// <summary>
    /// Clears the whole display.
    /// </summary>
    Everything = 0,

    /// <summary>
    /// Clears the whole display, including the
    /// scrollback buffer.
    /// </summary>
    EverythingAndScrollbackBuffer = 1,

    /// <summary>
    /// Clears everything before the cursor.
    /// </summary>
    BeforeCursor = 2,

    /// <summary>
    /// Clears everything after the cursor.
    /// </summary>
    AfterCursor = 3,
}
