using System;
using System.Text;

namespace Spectre.Terminals
{
    public interface ITerminalWriter
    {
        Encoding Encoding { get; }
        bool IsRedirected { get; }
        void Write(ReadOnlySpan<byte> buffer);
    }
}
