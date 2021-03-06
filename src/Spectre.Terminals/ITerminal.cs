namespace Spectre.Terminals;

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
    /// Occurs when a signal is received.
    /// </summary>
    event EventHandler<TerminalSignalEventArgs>? Signalled;

    /// <summary>
    /// Gets the terminal size.
    /// </summary>
    TerminalSize? Size { get; }

    /// <summary>
    /// Gets a <see cref="TerminalOutput"/> for <c>STDIN</c>.
    /// </summary>
    TerminalInput Input { get; }

    /// <summary>
    /// Gets a <see cref="TerminalOutput"/> for <c>STDOUT</c>.
    /// </summary>
    TerminalOutput Output { get; }

    /// <summary>
    /// Gets a <see cref="TerminalOutput"/> for <c>STDERR</c>.
    /// </summary>
    TerminalOutput Error { get; }

    /// <summary>
    /// Emits a signal.
    /// </summary>
    /// <param name="signal">The signal to emit.</param>
    /// <returns><c>true</c> if successful, otherwise false.</returns>
    bool EmitSignal(TerminalSignal signal);

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
