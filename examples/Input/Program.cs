using System;
using Spectre.Terminals;

namespace Input
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ReadLine();
            ReadKeys();
        }

        private static void ReadLine()
        {
            Terminal.Shared.Write("Write something> ");
            var line = Terminal.Shared.Input.ReadLine();
            Terminal.Shared.WriteLine($"Read = {line}");
        }

        private static void ReadKeys()
        {
            Terminal.Shared.WriteLine();
            Terminal.Shared.WriteLine("[Press any keys]");

            while (true)
            {
                // Read a key from the keyboard
                var key = Terminal.Shared.Input.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }

                // Get the character representation
                var character = !char.IsWhiteSpace(key.KeyChar)
                    ? key.KeyChar : '*';

                // Write to terminal
                Terminal.Shared.WriteLine(
                    $"{character} [KEY={key.Key} MOD={key.Modifiers}]");
            }
        }
    }
}
