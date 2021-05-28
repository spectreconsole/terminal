using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminal
{
    internal sealed class WindowsTerminalEmulatorAdapter : IWindowsTerminalWriter
    {
        private readonly IWindowsTerminalWriter _writer;
        private readonly WindowsTerminalEmulator _emulator;
        private readonly WindowsTerminalState _state;

        public SafeHandle Handle => _writer.Handle;
        public Encoding Encoding => _writer.Encoding;
        public bool IsRedirected => _writer.IsRedirected;

        public WindowsTerminalEmulatorAdapter(IWindowsTerminalWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _emulator = new WindowsTerminalEmulator();
            _state = new WindowsTerminalState(writer.Handle, writer);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool GetMode([NotNullWhen(true)] out CONSOLE_MODE? mode)
        {
            return _writer.GetMode(out mode);
        }

        public bool AddMode(CONSOLE_MODE mode)
        {
            return _writer.AddMode(mode);
        }

        public bool RemoveMode(CONSOLE_MODE mode)
        {
            return _writer.RemoveMode(mode);
        }

        public void Write(ReadOnlySpan<byte> buffer)
        {
            _emulator.Write(_state, buffer);
        }
    }
}
