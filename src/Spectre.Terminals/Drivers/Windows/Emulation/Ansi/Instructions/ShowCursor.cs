namespace Spectre.Terminals.Windows.Emulation
{
    internal sealed class ShowCursor : AnsiInstruction
    {
        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState state)
        {
            visitor.ShowCursor(this, state);
        }
    }
}
