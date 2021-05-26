namespace Spectre.Terminal.Ansi
{
    public sealed class CursorPosition : AnsiInstruction
    {
        public int Column { get; }
        public int Row { get; }

        public CursorPosition(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public override void Accept<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.CursorPosition(this, context);
        }
    }
}
