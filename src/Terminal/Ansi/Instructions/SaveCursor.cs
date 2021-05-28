namespace Spectre.Terminal.Ansi
{
    public sealed class SaveCursor : AnsiInstruction
    {
        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.SaveCursor(this, context);
        }
    }
}
