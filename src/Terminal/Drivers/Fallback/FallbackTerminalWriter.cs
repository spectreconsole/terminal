using System;
using System.IO;
using System.Text;

namespace Spectre.Terminal
{
    internal sealed class FallbackTerminalWriter : ITerminalWriter
    {
        private readonly ConsoleAnsiState _state;
        private readonly Func<bool> _redirected;

        public Encoding Encoding => Console.InputEncoding;
        public bool IsRedirected => _redirected();

        public FallbackTerminalWriter(Func<bool> redirected, Action<string?> writer)
        {
            _redirected = redirected;
            _state = new ConsoleAnsiState(writer);
        }

        public void Write(ReadOnlySpan<byte> buffer)
        {
            var text = Encoding.GetString(buffer);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            ConsoleAnsiInterpreter.Instance.Run(_state, text);
        }
    }
}
