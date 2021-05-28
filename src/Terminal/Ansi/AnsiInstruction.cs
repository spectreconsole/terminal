namespace Spectre.Terminal.Ansi
{
    public abstract class AnsiInstruction
    {
        public abstract void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context);
    }
}
