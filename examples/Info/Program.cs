using Spectre.Terminals;

namespace Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var terminal = Terminal.Shared;

            terminal.WriteLine();
            terminal.WriteLine("\u001b[38;5;15m\u001b[48;5;9mSpectre.Terminals\u001b[0m");
            terminal.WriteLine();
            terminal.WriteLine($"  Terminal driver = {terminal.Name}");
            terminal.WriteLine($"      Window size = {terminal.Size}");
            terminal.WriteLine($"Output redirected = {terminal.Output.IsRedirected}");
            terminal.WriteLine($"  Output encoding = {terminal.Output.Encoding.EncodingName}");
            terminal.WriteLine($" Error redirected = {terminal.Error.IsRedirected}");
            terminal.WriteLine($"   Error encoding = {terminal.Error.Encoding.EncodingName}");
            terminal.WriteLine($" Input redirected = {terminal.Input.IsRedirected}");
            terminal.WriteLine($"   Input encoding = {terminal.Input.Encoding.EncodingName}");
            terminal.WriteLine();

            WriteColors(terminal);
        }

        private static void WriteColors(ITerminal terminal)
        {
            terminal.WriteLine("Spectre.Terminals");
            for (var i = 0; i < 16; i++)
            {
                terminal.Write($"\u001b[38;5;{i}m\u001b[48;5;{i}m  \u001b[0m");
            }

            terminal.WriteLine();
            terminal.WriteLine("System.Console");
            for (var i = 0; i < 16; i++)
            {
                System.Console.BackgroundColor = GetColor(i);
                System.Console.Write("  ");
            }
            System.Console.ResetColor();
        }

        private static System.ConsoleColor GetColor(int number)
        {
            return number switch
            {
                0 => System.ConsoleColor.Black, // 0
                1 => System.ConsoleColor.DarkRed, // 4
                2 => System.ConsoleColor.DarkGreen, // 2
                3 => System.ConsoleColor.DarkYellow, // 6
                4 => System.ConsoleColor.DarkBlue, // 1
                5 => System.ConsoleColor.DarkMagenta, // 5
                6 => System.ConsoleColor.DarkCyan, // 3
                7 => System.ConsoleColor.Gray, // 7
                8 => System.ConsoleColor.DarkGray, // 8
                9 => System.ConsoleColor.Red, // 12
                10 => System.ConsoleColor.Green, // 10
                11 => System.ConsoleColor.Yellow, // 14
                12 => System.ConsoleColor.Blue, // 9
                13 => System.ConsoleColor.Magenta, // 13
                14 => System.ConsoleColor.Cyan, // 11
                15 => System.ConsoleColor.White, // 15
                _ => throw new System.InvalidOperationException("Cannot convert color to console color."),
            };
        }
    }
}
