namespace Spectre.Terminals.Emulation;

internal sealed class EnableAlternativeBuffer : AnsiInstruction
{
    public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState state)
    {
        visitor.EnableAlternativeBuffer(this, state);
    }
}
