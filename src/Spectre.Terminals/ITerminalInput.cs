namespace Spectre.Terminals
{
    /// <summary>
    /// Represents terminal input.
    /// </summary>
    public interface ITerminalInput : ITerminalReader
    {
        /// <summary>
        /// Redirects the input to a specific reader.
        /// </summary>
        /// <param name="reader">The reader to redirect to.</param>
        void Redirect(ITerminalReader? reader);
    }
}
