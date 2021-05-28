namespace Spectre.Terminal.Ansi
{
    public abstract class AnsiInstruction
    {
        public abstract void Accept<TContext>(IAnsiSequenceVisitor<TContext> visitor, TContext context);
    }
}
