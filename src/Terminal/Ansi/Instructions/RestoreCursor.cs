namespace Spectre.Terminals.Ansi
{
    public sealed class RestoreCursor : AnsiInstruction
    {
        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.RestoreCursor(this, context);
        }
    }
}
