using System;
using Spectre.Terminal;

namespace DotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var terminal = new Terminal(new FallbackTerminal());
            terminal.Output.Write("Hello World!");

            Console.ReadKey();

            terminal.Output.Write("Hello World!");
        }
    }
}
