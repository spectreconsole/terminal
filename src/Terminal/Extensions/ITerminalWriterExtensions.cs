using System;
using System.Buffers;

namespace Spectre.Terminals
{
    public static class ITerminalWriterExtensions
    {
        public static void Write(this ITerminalWriter writer, ReadOnlySpan<char> value)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));

            var len = writer.Encoding.GetByteCount(value);
            var array = ArrayPool<byte>.Shared.Rent(len);

            try
            {
                var span = array.AsSpan(0, len);
                writer.Encoding.GetBytes(value, span);
                writer.Write(span);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public static void Write(this ITerminalWriter writer, string? value)
        {
            Write(writer, value.AsSpan());
        }

        public static void WriteLine(this ITerminalWriter writer)
        {
            Write(writer, Environment.NewLine);
        }

        public static void WriteLine(this ITerminalWriter writer, string? value)
        {
            Write(writer, value.AsSpan());
            Write(writer, Environment.NewLine);
        }
    }
}
