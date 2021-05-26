namespace Spectre.Terminal.Ansi
{
    public sealed class SaveCursor : AnsiInstruction
    {
        public override void Accept<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.SaveCursor(this, context);
        }
    }
}
