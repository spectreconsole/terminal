namespace Spectre.Terminals.Emulation
{
    internal abstract class AnsiInstruction
    {
        public abstract void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState state);
    }
}
