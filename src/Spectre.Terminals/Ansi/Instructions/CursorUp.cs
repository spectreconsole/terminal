namespace Spectre.Terminals.Ansi
{
    public sealed class CursorUp : AnsiInstruction
    {
        public int Count { get; }

        public CursorUp(int count)
        {
            Count = count;
        }

        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.CursorUp(this, context);
        }
    }
}
