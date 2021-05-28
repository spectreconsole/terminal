namespace Spectre.Terminal.Ansi
{
    public sealed class CursorBack : AnsiInstruction
    {
        public int Count { get; }

        public CursorBack(int count)
        {
            Count = count;
        }

        public override void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorBack(this, context);
        }
    }
}
