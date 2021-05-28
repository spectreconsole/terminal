namespace Spectre.Terminal.Ansi
{
    public interface IAnsiSequenceVisitor<TState>
    {
        void CursorBack(CursorBack instruction, TState state);
        void CursorDown(CursorDown instruction, TState state);
        void CursorForward(CursorForward instruction, TState state);
        void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TState state);
        void CursorNextLine(CursorNextLine instruction, TState state);
        void CursorPosition(CursorPosition instruction, TState state);
        void CursorPreviousLine(CursorPreviousLine instruction, TState state);
        void CursorUp(CursorUp instruction, TState state);
        void EraseInDisplay(EraseInDisplay instruction, TState state);
        void EraseInLine(EraseInLine instruction, TState state);
        void PrintText(PrintText instruction, TState state);
        void RestoreCursor(RestoreCursor instruction, TState state);
        void SaveCursor(SaveCursor instruction, TState state);
    }
}