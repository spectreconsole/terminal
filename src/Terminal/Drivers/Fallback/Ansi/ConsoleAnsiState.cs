using System;

namespace Spectre.Terminal
{
    public sealed class ConsoleAnsiState
    {
        private readonly Action<string?> _writer;

        public ConsoleAnsiState(Action<string?> writer)
        {
            _writer = writer;
        }

        public void Write(ReadOnlySpan<char> text)
        {
            _writer(text.ToString());
        }
    }
}
