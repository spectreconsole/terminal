using System;
using System.IO;
using System.Threading.Tasks;

namespace Spectre.Terminals
{
    internal sealed class SynchronizedTextReader : TextReader
    {
        private readonly TextReader _inner;
        private readonly object _lock;

        public SynchronizedTextReader(TextReader reader)
        {
            _inner = reader ?? throw new ArgumentNullException(nameof(reader));
            _lock = new object();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }
        }

        public override int Peek()
        {
            lock (_lock)
            {
                return _inner.Peek();
            }
        }

        public override int Read()
        {
            lock (_lock)
            {
                return _inner.Peek();
            }
        }

        public override int Read(char[] buffer, int index, int count)
        {
            lock (_lock)
            {
                // TODO 2021-07-31: Validate input
                return _inner.Read(buffer, index, count);
            }
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            lock (_lock)
            {
                // TODO 2021-07-31: Validate input
                return _inner.ReadBlock(buffer, index, count);
            }
        }

        public override string? ReadLine()
        {
            lock (_lock)
            {
                return _inner.ReadLine();
            }
        }

        public override string ReadToEnd()
        {
            lock (_lock)
            {
                return _inner.ReadToEnd();
            }
        }

        public override Task<string?> ReadLineAsync()
        {
            return Task.FromResult(ReadLine());
        }

        public override Task<string> ReadToEndAsync()
        {
            return Task.FromResult(ReadToEnd());
        }

        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            // TODO 2021-07-31: Validate input
            return Task.FromResult(ReadBlock(buffer, index, count));
        }

        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            // TODO 2021-07-31: Validate input
            return Task.FromResult(Read(buffer, index, count));
        }
    }
}
