namespace Spectre.Terminal.Ansi
{
    public sealed class DisableAlternativeBuffer : AnsiInstruction
    {
        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState state)
        {
            visitor.DisableAlternativeBuffer(this, state);
        }
    }
}
