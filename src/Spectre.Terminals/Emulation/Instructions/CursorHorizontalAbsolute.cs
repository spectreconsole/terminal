namespace Spectre.Terminals.Emulation;

internal sealed class CursorHorizontalAbsolute : AnsiInstruction
{
    public int Column { get; }

    public CursorHorizontalAbsolute(int count)
    {
        Column = count;
    }

    public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
    {
        visitor.CursorHorizontalAbsolute(this, context);
    }
}
