namespace Spectre.Terminals
{
    /// <summary>
    /// Represents a terminal signal.
    /// </summary>
    public enum TerminalSignal
    {
        /// <summary>
        /// The SIGINT signal is sent to a process by its controlling terminal
        /// when a user wishes to interrupt the process.
        /// This is typically initiated by pressing Ctrl+C.
        /// </summary>
        SIGINT,

        /// <summary>
        /// The SIGQUIT signal is sent to a process by its controlling terminal
        /// when the user requests that the process quit.
        /// </summary>
        SIGQUIT,
    }
}
