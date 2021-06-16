using Spectre.Terminals;

namespace Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var terminal = Terminal.Shared;

            terminal.WriteLine();
            terminal.WriteLine("\u001b[38;5;14m\u001b[48;5;9mSpectre.Terminals\u001b[0m");
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
        }
    }
}
