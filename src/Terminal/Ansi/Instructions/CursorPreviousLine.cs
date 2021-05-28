namespace Spectre.Terminal.Ansi
{
    public sealed class CursorPreviousLine : AnsiInstruction
    {
        public int Count { get; }

        public CursorPreviousLine(int count)
        {
            Count = count;
        }

        public override void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorPreviousLine(this, context);
        }
    }
}
