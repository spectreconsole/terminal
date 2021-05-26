namespace Spectre.Terminal.Ansi
{
    public sealed class CursorDown : AnsiInstruction
    {
        public int Count { get; }

        public CursorDown(int count)
        {
            Count = count;
        }

        public override void Accept<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorDown(this, context);
        }
    }
}
