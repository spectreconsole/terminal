namespace Spectre.Terminals.Emulation;

internal sealed class PrintText : AnsiInstruction
{
    public ReadOnlyMemory<char> Text { get; }

    public PrintText(ReadOnlyMemory<char> text)
    {
        Text = text;
    }

    public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
    {
        visitor.PrintText(this, context);
    }
}
