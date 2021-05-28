using Spectre.Terminal;

namespace Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var terminal = Terminal.Shared;

            terminal.WriteLine("\u001b[2J\u001b[1;1HHello World!");
            terminal.WriteLine();

            terminal.WriteLine($"  Terminal driver = {terminal.Name}");
            terminal.WriteLine($"Output redirected = {terminal.Output.IsRedirected}");
            terminal.WriteLine($"  Output encoding = {terminal.Output.Encoding.EncodingName}");
            terminal.WriteLine($" Error redirected = {terminal.Error.IsRedirected}");
            terminal.WriteLine($"   Error encoding = {terminal.Error.Encoding.EncodingName}");
            terminal.WriteLine($" Input redirected = {terminal.Input.IsRedirected}");
            terminal.WriteLine($"   Input encoding = {terminal.Input.Encoding.EncodingName}");
            terminal.WriteLine();
            terminal.WriteLine("Press ANY key");
            terminal.WriteLine();
            terminal.ReadRaw();

            terminal.Write("\u001b[8;8HLOL!\u001b[0J");
            terminal.ReadRaw();

            terminal.Output.WriteLine("\u001b[2JGoodbye!");
        }
    }
}
