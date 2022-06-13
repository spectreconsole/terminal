namespace Spectre.Terminals;

/// <summary>
/// Provides data for the Signalled event.
/// </summary>
public sealed class TerminalSignalEventArgs : EventArgs
{
    /// <summary>
    /// Gets the signal.
    /// </summary>
    public TerminalSignal Signal { get; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the event was handled or not.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TerminalSignalEventArgs"/> class.
    /// </summary>
    /// <param name="signal">The signal.</param>
    public TerminalSignalEventArgs(TerminalSignal signal)
    {
        Signal = signal;
    }
}
