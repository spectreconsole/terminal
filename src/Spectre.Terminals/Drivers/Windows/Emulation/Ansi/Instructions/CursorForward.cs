namespace Spectre.Terminals.Windows.Emulation
{
    internal sealed class CursorForward : AnsiInstruction
    {
        public int Count { get; }

        public CursorForward(int count)
        {
            Count = count;
        }

        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.CursorForward(this, context);
        }
    }
}
