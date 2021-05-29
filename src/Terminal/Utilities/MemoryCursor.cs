using System;

namespace Spectre.Terminal
{
    internal sealed class MemoryCursor
    {
        private readonly ReadOnlyMemory<char> _buffer;
        private int _position;

        public bool CanRead => _position < _buffer.Length;
        public int Position => _position;

        public MemoryCursor(ReadOnlyMemory<char> buffer)
        {
            _buffer = buffer;
            _position = 0;
        }

        public int Peek()
        {
            if (_position >= _buffer.Length)
            {
                return -1;
            }

            return _buffer.Span[_position];
        }

        public char PeekChar()
        {
            return (char)Peek();
        }

        public char ReadChar()
        {
            return (char)Read();
        }

        public void Discard()
        {
            Read();
        }

        public void Discard(char expected)
        {
            var read = ReadChar();
            if (read != expected)
            {
                throw new InvalidOperationException($"Expected '{expected}' but got '{read}'.");
            }
        }

        public int Read()
        {
            var result = Peek();
            if (result != -1)
            {
                _position++;
            }

            return result;
        }

        public ReadOnlyMemory<char> Slice(int start, int stop)
        {
            return _buffer.Slice(start, stop - start);
        }
    }
}
