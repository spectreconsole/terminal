using System;
using System.Text;

namespace Spectre.Terminal
{
    public interface ITerminalReader
    {
        Encoding Encoding { get; }
        bool IsRedirected { get; }
        int Read(Span<byte> buffer);
    }
}
