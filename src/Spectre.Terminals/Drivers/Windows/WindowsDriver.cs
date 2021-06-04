using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Windows.Sdk;
using Spectre.Terminals.Windows.Emulation;

namespace Spectre.Terminals.Windows
{
    internal sealed class WindowsDriver : ITerminalDriver, IDisposable
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore")]
        private const CONSOLE_MODE IN_MODE = CONSOLE_MODE.ENABLE_PROCESSED_INPUT | CONSOLE_MODE.ENABLE_LINE_INPUT | CONSOLE_MODE.ENABLE_ECHO_INPUT;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore")]
        private const CONSOLE_MODE OUT_MODE = CONSOLE_MODE.DISABLE_NEWLINE_AUTO_RETURN;

        private readonly WindowsTerminalReader _input;
        private readonly IWindowsTerminalWriter _output;
        private readonly IWindowsTerminalWriter _error;
        private readonly WindowsSignals _signals;

        public string Name { get; } = "Windows";
        public bool IsRawMode { get; private set; }
        public TerminalSize? Size => GetTerminalSize();

        public event EventHandler<TerminalSignalEventArgs>? Signalled
        {
            add => _signals.Signalled += value;
            remove => _signals.Signalled += value;
        }

        ITerminalReader ITerminalDriver.Input => _input;
        ITerminalWriter ITerminalDriver.Output => _output;
        ITerminalWriter ITerminalDriver.Error => _error;

        public WindowsDriver(bool emulate = false)
        {
            _signals = new WindowsSignals();

            _input = new WindowsTerminalReader(this);
            _input.AddMode(
                CONSOLE_MODE.ENABLE_PROCESSED_INPUT |
                CONSOLE_MODE.ENABLE_LINE_INPUT |
                CONSOLE_MODE.ENABLE_ECHO_INPUT |
                CONSOLE_MODE.ENABLE_INSERT_MODE |
                CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_INPUT);

            _output = new WindowsTerminalWriter(STD_HANDLE_TYPE.STD_OUTPUT_HANDLE);
            _output.AddMode(
                CONSOLE_MODE.ENABLE_PROCESSED_OUTPUT |
                CONSOLE_MODE.ENABLE_WRAP_AT_EOL_OUTPUT |
                CONSOLE_MODE.DISABLE_NEWLINE_AUTO_RETURN |
                CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING);

            _error = new WindowsTerminalWriter(STD_HANDLE_TYPE.STD_ERROR_HANDLE);
            _error.AddMode(
                CONSOLE_MODE.ENABLE_PROCESSED_OUTPUT |
                CONSOLE_MODE.ENABLE_WRAP_AT_EOL_OUTPUT |
                CONSOLE_MODE.DISABLE_NEWLINE_AUTO_RETURN |
                CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING);

            // ENABLE_VIRTUAL_TERMINAL_PROCESSING not supported?
            if (emulate || (!(_output.GetMode(out var mode) && (mode & CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0)
                && !(_error.GetMode(out mode) && (mode & CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0)))
            {
                var colors = new WindowsColors(_output.Handle, _error.Handle);

                // Wrap STDOUT and STDERR in an emulator
                _output = new WindowsTerminalEmulatorAdapter(_output, colors);
                _error = new WindowsTerminalEmulatorAdapter(_error, colors);

                // Update the name to reflect the changes
                Name = "Windows (emulated)";
            }
        }

        public void Dispose()
        {
            _input.Dispose();
            _output.Dispose();
            _error.Dispose();
            _signals.Dispose();
        }

        public bool EmitSignal(TerminalSignal signal)
        {
            return _signals.Emit(signal);
        }

        public bool EnableRawMode()
        {
            // TODO: Restoration of mode when failed
            if (!(_input.RemoveMode(IN_MODE) && (_output.RemoveMode(OUT_MODE) || _error.RemoveMode(OUT_MODE))))
            {
                return false;
            }

            if (!PInvoke.FlushConsoleInputBuffer(_input.Handle))
            {
                throw new InvalidOperationException("Could not flush input buffer");
            }

            IsRawMode = true;
            return true;
        }

        public bool DisableRawMode()
        {
            // TODO: Restoration of mode when failed
            if (!(_input.AddMode(IN_MODE) && (_output.AddMode(OUT_MODE) || _error.AddMode(OUT_MODE))))
            {
                return false;
            }

            if (!PInvoke.FlushConsoleInputBuffer(_input.Handle))
            {
                throw new InvalidOperationException("Could not flush input buffer");
            }

            IsRawMode = false;
            return true;
        }

        private TerminalSize? GetTerminalSize()
        {
            if (!PInvoke.GetConsoleScreenBufferInfo(_output.Handle, out var info) &&
                !PInvoke.GetConsoleScreenBufferInfo(_error.Handle, out info) &&
                !PInvoke.GetConsoleScreenBufferInfo(_input.Handle, out info))
            {
                return null;
            }

            return new TerminalSize(info.srWindow.Right, info.srWindow.Bottom);
        }
    }
}
