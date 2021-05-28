namespace Spectre.Terminal.Ansi
{
    public sealed class CursorUp : AnsiInstruction
    {
        public int Count { get; }

        public CursorUp(int count)
        {
            Count = count;
        }

        public override void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorUp(this, context);
        }
    }
}
