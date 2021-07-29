using System;
using System.Text;

namespace Spectre.Terminals
{
    internal sealed class EncodingWithoutPreamble : Encoding
    {
        private readonly Encoding _encoding;

        public override string BodyName => _encoding.BodyName;

        public override int CodePage => _encoding.CodePage;

        public override string EncodingName => _encoding.EncodingName;

        public EncodingWithoutPreamble(Encoding encoding)
        {
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        public override byte[] GetPreamble()
        {
            return Array.Empty<byte>();
        }

        public override unsafe int GetByteCount(char* chars, int count)
        {
            return _encoding.GetByteCount(chars, count);
        }

        public override int GetByteCount(char[] chars)
        {
            return _encoding.GetByteCount(chars);
        }

        public override int GetByteCount(ReadOnlySpan<char> chars)
        {
            return _encoding.GetByteCount(chars);
        }

        public override int GetByteCount(string s)
        {
            return _encoding.GetByteCount(s);
        }

        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
        {
            return _encoding.GetBytes(chars, charCount, bytes, byteCount);
        }

        public override byte[] GetBytes(char[] chars)
        {
            return _encoding.GetBytes(chars);
        }

        public override byte[] GetBytes(char[] chars, int index, int count)
        {
            return _encoding.GetBytes(chars, index, count);
        }

        public override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            return _encoding.GetBytes(chars, bytes);
        }

        public override byte[] GetBytes(string s)
        {
            return _encoding.GetBytes(s);
        }

        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return _encoding.GetBytes(s, charIndex, charCount, bytes, byteIndex);
        }

        public override unsafe int GetCharCount(byte* bytes, int count)
        {
            return _encoding.GetCharCount(bytes, count);
        }

        public override int GetCharCount(byte[] bytes)
        {
            return _encoding.GetCharCount(bytes);
        }

        public override int GetCharCount(ReadOnlySpan<byte> bytes)
        {
            return _encoding.GetCharCount(bytes);
        }

        public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
        {
            return _encoding.GetChars(bytes, byteCount, chars, charCount);
        }

        public override char[] GetChars(byte[] bytes)
        {
            return _encoding.GetChars(bytes);
        }

        public override char[] GetChars(byte[] bytes, int index, int count)
        {
            return _encoding.GetChars(bytes, index, count);
        }

        public override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
        {
            return _encoding.GetChars(bytes, chars);
        }

        public override Decoder GetDecoder()
        {
            return _encoding.GetDecoder();
        }

        public override Encoder GetEncoder()
        {
            return _encoding.GetEncoder();
        }

        public override string GetString(byte[] bytes)
        {
            return _encoding.GetString(bytes);
        }

        public override bool IsAlwaysNormalized(NormalizationForm form)
        {
            return _encoding.IsAlwaysNormalized(form);
        }

        public override string GetString(byte[] bytes, int index, int count)
        {
            return _encoding.GetString(bytes, index, count);
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return _encoding.GetByteCount(chars, index, count);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return _encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return _encoding.GetCharCount(bytes, index, count);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return _encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        public override int GetMaxByteCount(int charCount)
        {
            return _encoding.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return _encoding.GetMaxByteCount(byteCount);
        }
    }
}
