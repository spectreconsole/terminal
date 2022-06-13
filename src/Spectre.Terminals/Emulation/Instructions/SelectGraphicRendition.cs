namespace Spectre.Terminals.Emulation;

internal sealed class SelectGraphicRendition : AnsiInstruction
{
    public IReadOnlyList<Operation> Operations { get; }

    internal sealed class Operation
    {
        public bool Reset { get; set; }
        public Color? Foreground { get; set; }
        public Color? Background { get; set; }
    }

    public SelectGraphicRendition(IEnumerable<Operation> ops)
    {
        Operations = ops as IReadOnlyList<Operation>
            ?? new List<Operation>(ops);
    }

    public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState state)
    {
        visitor.SelectGraphicRendition(this, state);
    }
}

internal readonly struct Color
{
    public int? Number { get; }
    public int? Red { get; }
    public int? Green { get; }
    public int? Blue { get; }

    public bool IsNumber => Number != null;
    public bool IsRgb => Red != null && Green != null && Blue != null;

    public Color(int number)
    {
        Number = number;
        Red = null;
        Green = null;
        Blue = null;
    }

    public Color(int red, int green, int blue)
    {
        Number = null;
        Red = red;
        Green = green;
        Blue = blue;
    }

    public override string ToString()
    {
        if (Number != null)
        {
            return Number.Value.ToString();
        }
        else if (Red != null && Green != null && Blue != null)
        {
            return $"{Red},{Green},{Blue}";
        }

        return "?";
    }
}
