namespace Spectre.Terminals.Emulation;

internal sealed class RestoreCursor : AnsiInstruction
{
    public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
    {
        visitor.RestoreCursor(this, context);
    }
}
