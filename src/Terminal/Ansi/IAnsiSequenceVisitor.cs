namespace Spectre.Terminal.Ansi
{
    public interface IAnsiSequenceVisitor<TContext>
    {
        void CursorBack(CursorBack instruction, TContext context);
        void CursorDown(CursorDown instruction, TContext context);
        void CursorForward(CursorForward instruction, TContext context);
        void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TContext context);
        void CursorNextLine(CursorNextLine instruction, TContext context);
        void CursorPosition(CursorPosition instruction, TContext context);
        void CursorPreviousLine(CursorPreviousLine instruction, TContext context);
        void CursorUp(CursorUp instruction, TContext context);
        void EraseInDisplay(EraseInDisplay instruction, TContext context);
        void EraseInLine(EraseInLine instruction, TContext context);
        void PrintText(PrintText instruction, TContext context);
        void RestoreCursor(RestoreCursor instruction, TContext context);
        void SaveCursor(SaveCursor instruction, TContext context);
    }
}
