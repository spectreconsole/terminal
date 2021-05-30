using System;
using System.Buffers;

namespace Spectre.Terminals
{
    /// <summary>
    /// Contains extension methods for <see cref="ITerminalWriter"/>.
    /// </summary>
    public static class ITerminalWriterExtensions
    {
        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value to write.</param>
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

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this ITerminalWriter writer, string? value)
        {
            Write(writer, value.AsSpan());
        }

        /// <summary>
        /// Writes an empty line.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public static void WriteLine(this ITerminalWriter writer)
        {
            Write(writer, Environment.NewLine);
        }

        /// <summary>
        /// Writes the specified text followed by a line break.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this ITerminalWriter writer, string? value)
        {
            Write(writer, value.AsSpan());
            Write(writer, Environment.NewLine);
        }
    }
}
