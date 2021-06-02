using System;
using System.Text;

namespace Spectre.Terminals
{
    /// <summary>
    /// Represents a reader.
    /// </summary>
    public interface ITerminalReader
    {
        /// <summary>
        /// Gets the encoding.
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the reader has been redirected.
        /// </summary>
        bool IsRedirected { get; }

        /// <summary>
        /// Reads a sequence of bytes from the current reader.
        /// </summary>
        /// <param name="buffer">
        /// A region of memory. When this method returns, the contents of this region are
        /// replaced by the bytes read from the current source.
        /// </param>
        /// <returns>
        /// The total number of bytes read into the buffer.
        /// This can be less than the number of bytes allocated in the buffer if
        /// that many bytes are not currently available, or zero (0) if the end
        /// of the stream has been reached.
        /// </returns>
        int Read(Span<byte> buffer);
    }
}
