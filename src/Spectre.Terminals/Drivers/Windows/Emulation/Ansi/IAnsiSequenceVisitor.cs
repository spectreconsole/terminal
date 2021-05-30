namespace Spectre.Terminals.Windows.Emulation
{
    /// <summary>
    /// Represents a VT/ANSI sequence visitor.
    /// </summary>
    /// <typeparam name="TState">The state.</typeparam>
    internal interface IAnsiSequenceVisitor<TState>
    {
        /// <summary>
        /// Handles a request to move the cursor backwards.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void CursorBack(CursorBack instruction, TState state);

        /// <summary>
        /// Handles a request to move the cursor down.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void CursorDown(CursorDown instruction, TState state);

        /// <summary>
        /// Handles a request to move the cursor forward.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void CursorForward(CursorForward instruction, TState state);

        /// <summary>
        /// Handles a request to set the cursor's horizontal position.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TState state);

        /// <summary>
        /// Handles a request to move the cursor to the next line
        /// and to set the cursor column to 0 (zero) position once done.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void CursorNextLine(CursorNextLine instruction, TState state);

        /// <summary>
        /// Handles a request to set the cursor position.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void CursorPosition(CursorPosition instruction, TState state);

        /// <summary>
        /// Handles a request to move the cursor to the previous line
        /// and to set the cursor column to 0 (zero) position once done.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void CursorPreviousLine(CursorPreviousLine instruction, TState state);

        /// <summary>
        /// Handles a request to move the cursor up.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void CursorUp(CursorUp instruction, TState state);

        /// <summary>
        /// Handles a request to clear the whole, or part of, the display.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void EraseInDisplay(EraseInDisplay instruction, TState state);

        /// <summary>
        /// Handles a request to clear the whole, or part of, the current line.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void EraseInLine(EraseInLine instruction, TState state);

        /// <summary>
        /// Handles a request to print text.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void PrintText(PrintText instruction, TState state);

        /// <summary>
        /// Handles a request to restore the cursor position to
        /// it's previously stored value.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void RestoreCursor(RestoreCursor instruction, TState state);

        /// <summary>
        /// Handles a request to store the cursor position.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void StoreCursor(StoreCursor instruction, TState state);

        /// <summary>
        /// Handles a request to hide the cursor.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void HideCursor(HideCursor instruction, TState state);

        /// <summary>
        /// Handles a request to show the cursor.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void ShowCursor(ShowCursor instruction, TState state);

        /// <summary>
        /// Handles a request to enable the alternate buffer.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void EnableAlternativeBuffer(EnableAlternativeBuffer instruction, TState state);

        /// <summary>
        /// Handles a request to disable the alternate buffer.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="state">The state.</param>
        void DisableAlternativeBuffer(DisableAlternativeBuffer instruction, TState state);
    }
}
