namespace Spectre.Terminal.Ansi
{
    public sealed class CursorHorizontalAbsolute : AnsiInstruction
    {
        public int Count { get; }

        public CursorHorizontalAbsolute(int count)
        {
            Count = count;
        }

        public override void Accept<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorHorizontalAbsolute(this, context);
        }
    }
}
