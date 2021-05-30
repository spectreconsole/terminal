using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminals.Windows
{
    /// <summary>
    /// Represents a Windows writer.
    /// </summary>
    internal interface IWindowsTerminalWriter : ITerminalWriter, IDisposable
    {
        /// <summary>
        /// Gets the handle.
        /// </summary>
        SafeHandle Handle { get; }

        /// <summary>
        /// Writes the specified data to the specified handle.
        /// </summary>
        /// <param name="handle">The handle to write to.</param>
        /// <param name="buffer">The buffer to write.</param>
        void Write(SafeHandle handle, ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Gets the <see cref="CONSOLE_MODE"/> for the writer.
        /// </summary>
        /// <param name="mode">The resulting <see cref="CONSOLE_MODE"/>, or <c>null</c> if the operation failed.</param>
        /// <returns><c>true</c> if the operation succeeded, otherwise <c>false</c>.</returns>
        bool GetMode([NotNullWhen(true)] out CONSOLE_MODE? mode);

        /// <summary>
        /// Adds the specified <see cref="CONSOLE_MODE"/>.
        /// </summary>
        /// <param name="mode">The <see cref="CONSOLE_MODE"/> to add.</param>
        /// <returns><c>true</c> if the operation succeeded, otherwise <c>false</c>.</returns>
        bool AddMode(CONSOLE_MODE mode);

        /// <summary>
        /// Removes the specified <see cref="CONSOLE_MODE"/>.
        /// </summary>
        /// <param name="mode">The <see cref="CONSOLE_MODE"/> to remove.</param>
        /// <returns><c>true</c> if the operation succeeded, otherwise <c>false</c>.</returns>
        bool RemoveMode(CONSOLE_MODE mode);
    }
}
