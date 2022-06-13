namespace Spectre.Terminals.Emulation;

internal sealed class CursorPreviousLine : AnsiInstruction
{
    public int Count { get; }

    public CursorPreviousLine(int count)
    {
        Count = count;
    }

    public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
    {
        visitor.CursorPreviousLine(this, context);
    }
}
