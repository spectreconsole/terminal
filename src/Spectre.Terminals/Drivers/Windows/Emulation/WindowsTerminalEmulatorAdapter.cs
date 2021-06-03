using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminals.Windows.Emulation
{
    internal sealed class WindowsTerminalEmulatorAdapter : IWindowsTerminalWriter
    {
        private readonly IWindowsTerminalWriter _writer;
        private readonly WindowsTerminalEmulator _emulator;
        private readonly WindowsTerminalState _state;

        public SafeHandle Handle => _writer.Handle;

        public string Name => _writer.Name;

        public Encoding Encoding
        {
            get => _writer.Encoding;
            set => _writer.Encoding = value;
        }

        public bool IsRedirected => _writer.IsRedirected;

        public WindowsTerminalEmulatorAdapter(IWindowsTerminalWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _emulator = new WindowsTerminalEmulator();
            _state = new WindowsTerminalState(writer);
        }

        public void Dispose()
        {
            _writer.Dispose();
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
            if (_writer.IsRedirected)
            {
                _writer.Write(Handle, buffer);
            }
            else
            {
                _emulator.Write(_state, buffer);
            }
        }

        public void Write(SafeHandle handle, ReadOnlySpan<byte> buffer)
        {
            throw new NotSupportedException();
        }
    }
}
