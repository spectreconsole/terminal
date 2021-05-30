using Spectre.Terminals;

namespace Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var terminal = Terminal.Shared;

            // Information
            terminal.WriteLine("\u001b[2J\u001b[1;1HSpectre.Terminal");
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

            // Do some line manipulation
            //terminal.Write("\u001b[6;8H[Delete after]\u001b[0K");
            //terminal.ReadRaw();
            //terminal.Write("\u001b[5;15H\u001b[1K[Delete before]");
            //terminal.ReadRaw();
            //terminal.Write("\u001b[4;15H\u001b[2K[Delete line]");
            //terminal.ReadRaw();

            // Write some text in an alternate buffer
            terminal.Write("\u001b[?1049h");
            terminal.WriteLine("HELLO WORLD!");
            terminal.ReadRaw();
            terminal.Write("\u001b[?1049l");
            terminal.Write("");

            terminal.Output.WriteLine("Goodbye!");
        }
    }
}
