namespace Spectre.Terminal.Ansi
{
    public sealed class EraseInLine : AnsiInstruction
    {
        public int Mode { get; }

        public EraseInLine(int mode)
        {
            Mode = mode;
        }

        public override void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.EraseInLine(this, context);
        }
    }
}
