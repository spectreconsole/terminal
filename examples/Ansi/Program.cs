using System;
using Spectre.Terminal;

namespace Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var terminal = Terminal.Instance;

            terminal.WriteLine("\u001b[2J\u001b[1;1HHello World!");
            terminal.WriteLine();

            terminal.WriteLine($"Output redirected = {terminal.Output.IsRedirected}");
            terminal.WriteLine($"  Output encoding = {terminal.Output.Encoding.EncodingName}");
            terminal.WriteLine($" Error redirected = {terminal.Error.IsRedirected}");
            terminal.WriteLine($"   Error encoding = {terminal.Error.Encoding.EncodingName}");
            terminal.WriteLine($" Input redirected = {terminal.Input.IsRedirected}");
            terminal.WriteLine($"   Input encoding = {terminal.Input.Encoding.EncodingName}");

            terminal.EnableRawMode();
            terminal.Input.ReadRaw();
            terminal.DisableRawMode();

            terminal.Output.WriteLine("Goodbye!");
        }
    }
}
