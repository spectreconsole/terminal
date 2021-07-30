using System;

#if NET5_0_OR_GREATER
using System.Buffers;
#endif

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
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

#if NET5_0_OR_GREATER
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
#else
            var chars = value.ToArray();
            var bytes = writer.Encoding.GetBytes(chars);
            writer.Write(new Span<byte>(bytes));
#endif
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
