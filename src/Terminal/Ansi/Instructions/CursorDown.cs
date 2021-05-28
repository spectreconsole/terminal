namespace Spectre.Terminal.Ansi
{
    public sealed class CursorDown : AnsiInstruction
    {
        public int Count { get; }

        public CursorDown(int count)
        {
            Count = count;
        }

        public override void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorDown(this, context);
        }
    }
}
