using System;
using System.Text;

namespace Spectre.Terminals
{
    public interface ITerminalReader
    {
        Encoding Encoding { get; }
        bool IsRedirected { get; }
        int Read(Span<byte> buffer);
    }
}
