namespace Spectre.Terminal.Ansi
{
    public sealed class SaveCursor : AnsiInstruction
    {
        public override void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.SaveCursor(this, context);
        }
    }
}
