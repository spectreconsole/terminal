namespace Spectre.Terminals
{
    /// <summary>
    /// Represents terminal input.
    /// </summary>
    public interface ITerminalOutput : ITerminalWriter
    {
        /// <summary>
        /// Redirects the input to a specific writer.
        /// </summary>
        /// <param name="reader">The writer to redirect to.</param>
        void Redirect(ITerminalWriter? reader);
    }
}
