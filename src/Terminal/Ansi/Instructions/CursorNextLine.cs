namespace Spectre.Terminal.Ansi
{
    public sealed class CursorNextLine : AnsiInstruction
    {
        public int Count { get; }

        public CursorNextLine(int count)
        {
            Count = count;
        }

        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.CursorNextLine(this, context);
        }
    }
}
