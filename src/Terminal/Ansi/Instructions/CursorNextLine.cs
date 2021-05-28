namespace Spectre.Terminal.Ansi
{
    public sealed class CursorNextLine : AnsiInstruction
    {
        public int Count { get; }

        public CursorNextLine(int count)
        {
            Count = count;
        }

        public override void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorNextLine(this, context);
        }
    }
}
