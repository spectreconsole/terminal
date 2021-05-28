namespace Spectre.Terminal.Ansi
{
    public sealed class EraseInLine : AnsiInstruction
    {
        public int Mode { get; }

        public EraseInLine(int mode)
        {
            Mode = mode;
        }

        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.EraseInLine(this, context);
        }
    }
}
