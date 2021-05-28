using System;
using System.Text;

namespace Spectre.Terminal
{
    public interface ITerminalWriter
    {
        Encoding Encoding { get; }
        bool IsRedirected { get; }
        void Write(ReadOnlySpan<byte> buffer);
    }
}
