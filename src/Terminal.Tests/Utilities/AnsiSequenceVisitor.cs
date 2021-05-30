namespace Spectre.Terminal.Ansi
{
    public abstract class AnsiSequenceVisitor<TContext> : IAnsiSequenceVisitor<TContext>
    {
        void IAnsiSequenceVisitor<TContext>.CursorBack(CursorBack instruction, TContext state) => CursorBack(instruction, state);
        void IAnsiSequenceVisitor<TContext>.CursorDown(CursorDown instruction, TContext state) => CursorDown(instruction, state);
        void IAnsiSequenceVisitor<TContext>.CursorForward(CursorForward instruction, TContext state) => CursorForward(instruction, state);
        void IAnsiSequenceVisitor<TContext>.CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TContext state) => CursorHorizontalAbsolute(instruction, state);
        void IAnsiSequenceVisitor<TContext>.CursorNextLine(CursorNextLine instruction, TContext state) => CursorNextLine(instruction, state);
        void IAnsiSequenceVisitor<TContext>.CursorPosition(CursorPosition instruction, TContext state) => CursorPosition(instruction, state);
        void IAnsiSequenceVisitor<TContext>.CursorPreviousLine(CursorPreviousLine instruction, TContext state) => CursorPreviousLine(instruction, state);
        void IAnsiSequenceVisitor<TContext>.CursorUp(CursorUp instruction, TContext state) => CursorUp(instruction, state);
        void IAnsiSequenceVisitor<TContext>.EraseInDisplay(EraseInDisplay instruction, TContext state) => EraseInDisplay(instruction, state);
        void IAnsiSequenceVisitor<TContext>.EraseInLine(EraseInLine instruction, TContext state) => EraseInLine(instruction, state);
        void IAnsiSequenceVisitor<TContext>.PrintText(PrintText instruction, TContext state) => PrintText(instruction, state);
        void IAnsiSequenceVisitor<TContext>.RestoreCursor(RestoreCursor instruction, TContext state) => RestoreCursor(instruction, state);
        void IAnsiSequenceVisitor<TContext>.SaveCursor(SaveCursor instruction, TContext state) => SaveCursor(instruction, state);
        void IAnsiSequenceVisitor<TContext>.HideCursor(HideCursor instruction, TContext state) => HideCursor(instruction, state);
        void IAnsiSequenceVisitor<TContext>.ShowCursor(ShowCursor instruction, TContext state) => ShowCursor(instruction, state);

        protected virtual void CursorBack(CursorBack instruction, TContext state)
        {
        }

        protected virtual void CursorDown(CursorDown instruction, TContext state)
        {
        }

        protected virtual void CursorForward(CursorForward instruction, TContext state)
        {
        }

        protected virtual void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TContext state)
        {
        }

        protected virtual void CursorNextLine(CursorNextLine instruction, TContext state)
        {
        }

        protected virtual void CursorPosition(CursorPosition instruction, TContext state)
        {
        }

        protected virtual void CursorPreviousLine(CursorPreviousLine instruction, TContext state)
        {
        }

        protected virtual void CursorUp(CursorUp instruction, TContext state)
        {
        }

        protected virtual void EraseInDisplay(EraseInDisplay instruction, TContext state)
        {
        }

        protected virtual void EraseInLine(EraseInLine instruction, TContext state)
        {
        }

        protected virtual void PrintText(PrintText instruction, TContext state)
        {
        }

        protected virtual void RestoreCursor(RestoreCursor instruction, TContext state)
        {
        }

        protected virtual void SaveCursor(SaveCursor instruction, TContext state)
        {
        }

        protected virtual void HideCursor(HideCursor instruction, TContext state)
        {
        }

        protected virtual void ShowCursor(ShowCursor instruction, TContext state)
        {
        }
    }
}
