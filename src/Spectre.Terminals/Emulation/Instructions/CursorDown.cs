namespace Spectre.Terminals.Emulation;

internal sealed class CursorDown : AnsiInstruction
{
    public int Count { get; }

    public CursorDown(int count)
    {
        Count = count;
    }

    public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
    {
        visitor.CursorDown(this, context);
    }
}
