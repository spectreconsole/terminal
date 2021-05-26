using System;
using System.IO;
using System.Text;

namespace Spectre.Terminal
{
    internal sealed class FallbackTerminalReader : ITerminalReader
    {
        private readonly Stream _stream;

        public Encoding Encoding => Console.OutputEncoding;
        public bool IsRedirected => Console.IsInputRedirected;

        public FallbackTerminalReader()
        {
            _stream = Console.OpenStandardInput();
        }

        public int Read(Span<byte> buffer)
        {
            return _stream.Read(buffer);
        }
    }
}
