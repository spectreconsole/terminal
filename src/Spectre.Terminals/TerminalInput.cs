using System;
using System.Text;

namespace Spectre.Terminals
{
    internal sealed class TerminalInput : ITerminalReader
    {
        private readonly ITerminalReader _reader;
        private readonly object _lock;
        private ITerminalReader? _redirected;

        public Encoding Encoding => GetEncoding();
        public bool IsRedirected => GetIsRedirected();

        public TerminalInput(ITerminalReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _lock = new object();
        }

        public void Redirect(ITerminalReader? reader)
        {
            lock (_lock)
            {
                _redirected = reader;
            }
        }

        public int Read(Span<byte> buffer)
        {
            lock (_lock)
            {
                if (_redirected != null)
                {
                    return _redirected.Read(buffer);
                }

                return _reader.Read(buffer);
            }
        }

        private Encoding GetEncoding()
        {
            lock (_lock)
            {
                if (_redirected != null)
                {
                    return _redirected.Encoding;
                }

                return _reader.Encoding;
            }
        }

        private bool GetIsRedirected()
        {
            lock (_lock)
            {
                if (_redirected != null)
                {
                    return _redirected.IsRedirected;
                }

                return _reader.IsRedirected;
            }
        }
    }
}
