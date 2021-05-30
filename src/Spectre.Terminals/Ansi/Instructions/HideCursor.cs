namespace Spectre.Terminals.Ansi
{
    public sealed class HideCursor : AnsiInstruction
    {
        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState state)
        {
            visitor.HideCursor(this, state);
        }
    }
}
