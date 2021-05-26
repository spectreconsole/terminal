namespace Spectre.Terminal.Ansi
{
    public abstract class AnsiInstruction
    {
        public abstract void Accept<TContext>(AnsiSequenceVisitor<TContext> visitor, TContext context);
    }
}
