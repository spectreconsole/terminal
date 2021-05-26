namespace Spectre.Terminal.Ansi
{
    public sealed class EraseInDisplay : AnsiInstruction
    {
        public int Mode { get; }

        public EraseInDisplay(int mode)
        {
            Mode = mode;
        }

        public override void Accept<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context)
        {
            visitor.EraseInDisplay(this, context);
        }
    }
}
