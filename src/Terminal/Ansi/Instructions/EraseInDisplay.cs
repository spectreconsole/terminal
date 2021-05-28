namespace Spectre.Terminal.Ansi
{
    public sealed class EraseInDisplay : AnsiInstruction
    {
        public int Mode { get; }

        public EraseInDisplay(int mode)
        {
            Mode = mode;
        }

        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState context)
        {
            visitor.EraseInDisplay(this, context);
        }
    }
}
