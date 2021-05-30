namespace Spectre.Terminals.Ansi
{
    public sealed class StoreCursor : AnsiInstruction
    {
        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.StoreCursor(this, context);
        }
    }
}
