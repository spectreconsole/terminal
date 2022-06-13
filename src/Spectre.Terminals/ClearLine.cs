namespace Spectre.Terminals;

/// <summary>
/// Represents different ways of clearing a line.
/// </summary>
public enum ClearLine
{
    /// <summary>
    /// Clears the whole line.
    /// </summary>
    WholeLine = 0,

    /// <summary>
    /// Clears everything before the cursor.
    /// </summary>
    BeforeCursor = 1,

    /// <summary>
    /// Clears everything after the cursor.
    /// </summary>
    AfterCursor = 2,
}
