namespace Spectre.Terminals.Emulation
{
    internal sealed class CursorPosition : AnsiInstruction
    {
        public int Column { get; }
        public int Row { get; }

        public CursorPosition(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.CursorPosition(this, context);
        }
    }
}
