namespace Spectre.Terminal.Ansi
{
    public abstract class AnsiSequenceVisitor<TContext>
    {
        protected internal virtual void CursorBack(CursorBack instruction, TContext context)
        {
        }

        protected internal virtual void CursorDown(CursorDown instruction, TContext context)
        {
        }

        protected internal virtual void CursorForward(CursorForward instruction, TContext context)
        {
        }

        protected internal virtual void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TContext context)
        {
        }

        protected internal virtual void CursorNextLine(CursorNextLine instruction, TContext context)
        {
        }

        protected internal virtual void CursorPosition(CursorPosition instruction, TContext context)
        {
        }

        protected internal virtual void CursorPreviousLine(CursorPreviousLine instruction, TContext context)
        {
        }

        protected internal virtual void CursorUp(CursorUp instruction, TContext context)
        {
        }

        protected internal virtual void EraseInDisplay(EraseInDisplay instruction, TContext context)
        {
        }

        protected internal virtual void EraseInLine(EraseInLine instruction, TContext context)
        {
        }

        protected internal virtual void PrintText(PrintText instruction, TContext context)
        {
        }

        protected internal virtual void RestoreCursor(RestoreCursor instruction, TContext context)
        {
        }

        protected internal virtual void SaveCursor(SaveCursor instruction, TContext context)
        {
        }
    }
}
