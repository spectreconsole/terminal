using System;

namespace Spectre.Terminals
{
    /// <summary>
    /// Represents a terminal.
    /// </summary>
    public interface ITerminal : IDisposable
    {
        /// <summary>
        /// Gets the name of the terminal driver.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether or not the terminal is in raw mode.
        /// </summary>
        bool IsRawMode { get; }

        /// <summary>
        /// Gets a <see cref="ITerminalWriter"/> for <c>STDIN</c>.
        /// </summary>
        ITerminalReader Input { get; }

        /// <summary>
        /// Gets a <see cref="ITerminalWriter"/> for <c>STDOUT</c>.
        /// </summary>
        ITerminalWriter Output { get; }

        /// <summary>
        /// Gets a <see cref="ITerminalWriter"/> for <c>STDERR</c>.
        /// </summary>
        ITerminalWriter Error { get; }

        /// <summary>
        /// Enables raw mode.
        /// </summary>
        /// <returns><c>true</c> if the operation succeeded, otherwise <c>false</c>.</returns>
        bool EnableRawMode();

        /// <summary>
        /// Disables raw mode.
        /// </summary>
        /// <returns><c>true</c> if the operation succeeded, otherwise <c>false</c>.</returns>
        bool DisableRawMode();
    }
}
