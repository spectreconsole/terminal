namespace Spectre.Terminals.Windows.Emulation
{
    internal abstract class AnsiInstruction
    {
        public abstract void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState state);
    }
}
