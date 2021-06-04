using Spectre.Terminals.Windows.Emulation;

namespace Spectre.Terminals.Tests
{
    internal abstract class AnsiSequenceVisitor<TState> : IAnsiSequenceVisitor<TState>
    {
        void IAnsiSequenceVisitor<TState>.CursorBack(CursorBack instruction, TState state) => CursorBack(instruction, state);
        void IAnsiSequenceVisitor<TState>.CursorDown(CursorDown instruction, TState state) => CursorDown(instruction, state);
        void IAnsiSequenceVisitor<TState>.CursorForward(CursorForward instruction, TState state) => CursorForward(instruction, state);
        void IAnsiSequenceVisitor<TState>.CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TState state) => CursorHorizontalAbsolute(instruction, state);
        void IAnsiSequenceVisitor<TState>.CursorNextLine(CursorNextLine instruction, TState state) => CursorNextLine(instruction, state);
        void IAnsiSequenceVisitor<TState>.CursorPosition(CursorPosition instruction, TState state) => CursorPosition(instruction, state);
        void IAnsiSequenceVisitor<TState>.CursorPreviousLine(CursorPreviousLine instruction, TState state) => CursorPreviousLine(instruction, state);
        void IAnsiSequenceVisitor<TState>.CursorUp(CursorUp instruction, TState state) => CursorUp(instruction, state);
        void IAnsiSequenceVisitor<TState>.EraseInDisplay(EraseInDisplay instruction, TState state) => EraseInDisplay(instruction, state);
        void IAnsiSequenceVisitor<TState>.EraseInLine(EraseInLine instruction, TState state) => EraseInLine(instruction, state);
        void IAnsiSequenceVisitor<TState>.PrintText(PrintText instruction, TState state) => PrintText(instruction, state);
        void IAnsiSequenceVisitor<TState>.RestoreCursor(RestoreCursor instruction, TState state) => RestoreCursor(instruction, state);
        void IAnsiSequenceVisitor<TState>.StoreCursor(StoreCursor instruction, TState state) => StoreCursor(instruction, state);
        void IAnsiSequenceVisitor<TState>.HideCursor(HideCursor instruction, TState state) => HideCursor(instruction, state);
        void IAnsiSequenceVisitor<TState>.ShowCursor(ShowCursor instruction, TState state) => ShowCursor(instruction, state);
        void IAnsiSequenceVisitor<TState>.EnableAlternativeBuffer(EnableAlternativeBuffer instruction, TState state) => EnableAlternativeBuffer(instruction, state);
        void IAnsiSequenceVisitor<TState>.DisableAlternativeBuffer(DisableAlternativeBuffer instruction, TState state) => DisableAlternativeBuffer(instruction, state);
        void IAnsiSequenceVisitor<TState>.SelectGraphicRendition(SelectGraphicRendition instruction, TState state) => SelectGraphicRendition(instruction, state);

        protected virtual void CursorBack(CursorBack instruction, TState state)
        {
        }

        protected virtual void CursorDown(CursorDown instruction, TState state)
        {
        }

        protected virtual void CursorForward(CursorForward instruction, TState state)
        {
        }

        protected virtual void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, TState state)
        {
        }

        protected virtual void CursorNextLine(CursorNextLine instruction, TState state)
        {
        }

        protected virtual void CursorPosition(CursorPosition instruction, TState state)
        {
        }

        protected virtual void CursorPreviousLine(CursorPreviousLine instruction, TState state)
        {
        }

        protected virtual void CursorUp(CursorUp instruction, TState state)
        {
        }

        protected virtual void EraseInDisplay(EraseInDisplay instruction, TState state)
        {
        }

        protected virtual void EraseInLine(EraseInLine instruction, TState state)
        {
        }

        protected virtual void PrintText(PrintText instruction, TState state)
        {
        }

        protected virtual void RestoreCursor(RestoreCursor instruction, TState state)
        {
        }

        protected virtual void StoreCursor(StoreCursor instruction, TState state)
        {
        }

        protected virtual void HideCursor(HideCursor instruction, TState state)
        {
        }

        protected virtual void ShowCursor(ShowCursor instruction, TState state)
        {
        }

        protected virtual void EnableAlternativeBuffer(EnableAlternativeBuffer instruction, TState state)
        {
        }

        protected virtual void DisableAlternativeBuffer(DisableAlternativeBuffer instruction, TState state)
        {
        }

        protected virtual void SelectGraphicRendition(SelectGraphicRendition instruction, TState state)
        {
        }
    }
}
