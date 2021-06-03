using System;
using System.Text;

namespace Spectre.Terminals
{
    internal sealed class TerminalOutput : ITerminalWriter
    {
        private readonly ITerminalWriter _writer;
        private readonly object _lock;
        private ITerminalWriter? _redirected;

        public Encoding Encoding
        {
            get => GetEncoding();
            set => SetEncoding(value);
        }

        public bool IsRedirected => GetIsRedirected();

        public TerminalOutput(ITerminalWriter reader)
        {
            _writer = reader ?? throw new ArgumentNullException(nameof(reader));
            _lock = new object();
        }

        public void Redirect(ITerminalWriter? writer)
        {
            lock (_lock)
            {
                _redirected = writer;
            }
        }

        public void Write(ReadOnlySpan<byte> buffer)
        {
            lock (_lock)
            {
                if (_redirected != null)
                {
                    _redirected.Write(buffer);
                }
                else
                {
                    _writer.Write(buffer);
                }
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

                return _writer.Encoding;
            }
        }

        private void SetEncoding(Encoding encoding)
        {
            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            lock (_lock)
            {
                if (_redirected != null)
                {
                    _redirected.Encoding = encoding;
                }
                else
                {
                    _writer.Encoding = encoding;
                }
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

                return _writer.IsRedirected;
            }
        }
    }
}
