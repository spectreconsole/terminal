namespace Spectre.Terminal.Ansi
{
    public sealed class RestoreCursor : AnsiInstruction
    {
        public override void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.RestoreCursor(this, context);
        }
    }
}
