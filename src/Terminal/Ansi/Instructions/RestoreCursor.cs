namespace Spectre.Terminal.Ansi
{
    public sealed class RestoreCursor : AnsiInstruction
    {
        public override void Accept<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.RestoreCursor(this, context);
        }
    }
}
