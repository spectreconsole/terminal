namespace Spectre.Terminal.Ansi
{
    public sealed class CursorForward : AnsiInstruction
    {
        public int Count { get; }

        public CursorForward(int count)
        {
            Count = count;
        }

        public override void Accept<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorForward(this, context);
        }
    }
}
