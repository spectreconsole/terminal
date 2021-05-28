namespace Spectre.Terminal.Ansi
{
    public abstract class AnsiSequenceVisitor<TContext> : IAnsiSequenceVisitor<TContext>
    {
        void IAnsiSequenceVisitor<TContext>.CursorBack(CursorBack instruction, TContext context) => CursorBack(instruction, context);
        void IAnsiSequenceVisitor<TContext>.CursorDown(CursorDown instruction, TContext context) => CursorDown(instruction, context);
        void IAnsiSequenceVisitor<TContext>.CursorForward(CursorForward instruction, TContext context) => CursorForward(instruction, context);
        void IAnsiSequenceVisitor<TContext>.CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TContext context) => CursorHorizontalAbsolute(instruction, context);
        void IAnsiSequenceVisitor<TContext>.CursorNextLine(CursorNextLine instruction, TContext context) => CursorNextLine(instruction, context);
        void IAnsiSequenceVisitor<TContext>.CursorPosition(CursorPosition instruction, TContext context) => CursorPosition(instruction, context);
        void IAnsiSequenceVisitor<TContext>.CursorPreviousLine(CursorPreviousLine instruction, TContext context) => CursorPreviousLine(instruction, context);
        void IAnsiSequenceVisitor<TContext>.CursorUp(CursorUp instruction, TContext context) => CursorUp(instruction, context);
        void IAnsiSequenceVisitor<TContext>.EraseInDisplay(EraseInDisplay instruction, TContext context) => EraseInDisplay(instruction, context);
        void IAnsiSequenceVisitor<TContext>.EraseInLine(EraseInLine instruction, TContext context) => EraseInLine(instruction, context);
        void IAnsiSequenceVisitor<TContext>.PrintText(PrintText instruction, TContext context) => PrintText(instruction, context);
        void IAnsiSequenceVisitor<TContext>.RestoreCursor(RestoreCursor instruction, TContext context) => RestoreCursor(instruction, context);
        void IAnsiSequenceVisitor<TContext>.SaveCursor(SaveCursor instruction, TContext context) => SaveCursor(instruction, context);

        protected virtual void CursorBack(CursorBack instruction, TContext context)
        {
        }

        protected virtual void CursorDown(CursorDown instruction, TContext context)
        {
        }

        protected virtual void CursorForward(CursorForward instruction, TContext context)
        {
        }

        protected virtual void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TContext context)
        {
        }

        protected virtual void CursorNextLine(CursorNextLine instruction, TContext context)
        {
        }

        protected virtual void CursorPosition(CursorPosition instruction, TContext context)
        {
        }

        protected virtual void CursorPreviousLine(CursorPreviousLine instruction, TContext context)
        {
        }

        protected virtual void CursorUp(CursorUp instruction, TContext context)
        {
        }

        protected virtual void EraseInDisplay(EraseInDisplay instruction, TContext context)
        {
        }

        protected virtual void EraseInLine(EraseInLine instruction, TContext context)
        {
        }

        protected virtual void PrintText(PrintText instruction, TContext context)
        {
        }

        protected virtual void RestoreCursor(RestoreCursor instruction, TContext context)
        {
        }

        protected virtual void SaveCursor(SaveCursor instruction, TContext context)
        {
        }
    }
}
