namespace Spectre.Terminals.Ansi
{
    public sealed class CursorBack : AnsiInstruction
    {
        public int Count { get; }

        public CursorBack(int count)
        {
            Count = count;
        }

        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.CursorBack(this, context);
        }
    }
}
