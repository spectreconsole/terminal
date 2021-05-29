using System;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminal
{
    internal sealed class WindowsDriver : ITerminalDriver, IDisposable
    {
        private const CONSOLE_MODE IN_MODE = CONSOLE_MODE.ENABLE_PROCESSED_INPUT | CONSOLE_MODE.ENABLE_LINE_INPUT | CONSOLE_MODE.ENABLE_ECHO_INPUT;
        private const CONSOLE_MODE OUT_MODE = CONSOLE_MODE.DISABLE_NEWLINE_AUTO_RETURN;

        public string Name { get; } = "Windows";
        public bool IsRawMode { get; private set; }

        public WindowsTerminalReader Input { get; }
        public IWindowsTerminalWriter Output { get; }
        public IWindowsTerminalWriter Error { get; }

        ITerminalReader ITerminalDriver.Input => Input;
        ITerminalWriter ITerminalDriver.Output => Output;
        ITerminalWriter ITerminalDriver.Error => Error;

        public WindowsDriver(bool emulate = false)
        {
            Input = new WindowsTerminalReader(this);
            Input.AddMode(
                CONSOLE_MODE.ENABLE_PROCESSED_INPUT |
                CONSOLE_MODE.ENABLE_LINE_INPUT |
                CONSOLE_MODE.ENABLE_ECHO_INPUT |
                CONSOLE_MODE.ENABLE_INSERT_MODE |
                CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_INPUT);

            Output = new WindowsTerminalWriter(STD_HANDLE_TYPE.STD_OUTPUT_HANDLE);
            Output.AddMode(
                CONSOLE_MODE.ENABLE_PROCESSED_OUTPUT |
                CONSOLE_MODE.ENABLE_WRAP_AT_EOL_OUTPUT |
                CONSOLE_MODE.DISABLE_NEWLINE_AUTO_RETURN |
                CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING);
             
            Error = new WindowsTerminalWriter(STD_HANDLE_TYPE.STD_ERROR_HANDLE);
            Error.AddMode(
                CONSOLE_MODE.ENABLE_PROCESSED_OUTPUT |
                CONSOLE_MODE.ENABLE_WRAP_AT_EOL_OUTPUT |
                CONSOLE_MODE.DISABLE_NEWLINE_AUTO_RETURN |
                CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING);

            // ANSI not supported?
            if (emulate || (!(Output.GetMode(out var mode) && (mode & CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0)
                && !(Error.GetMode(out mode) && (mode & CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0)))
            {
                // Wrap STDOUT and STDERR in an emulator
                Name = "Windows (emulated)";
                Output = new WindowsTerminalEmulatorAdapter(Output);
                Error = new WindowsTerminalEmulatorAdapter(Error);
            }
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            Error.Dispose();
        }

        public bool EnableRawMode()
        {
            // TODO: Restoration of mode when failed
            if (!(Input.RemoveMode(IN_MODE) && (Output.RemoveMode(OUT_MODE) || Error.RemoveMode(OUT_MODE))))
            {
                return false;
            }

            if (!PInvoke.FlushConsoleInputBuffer(Input.Handle))
            {
                throw new InvalidOperationException("Could not flush input buffer");
            }

            IsRawMode = true;
            return true;
        }

        public bool DisableRawMode()
        {
            // TODO: Restoration of mode when failed
            if (!(Input.AddMode(IN_MODE) && (Output.AddMode(OUT_MODE) || Error.AddMode(OUT_MODE))))
            {
                return false;
            }

            if (!PInvoke.FlushConsoleInputBuffer(Input.Handle))
            {
                throw new InvalidOperationException("Could not flush input buffer");
            }

            IsRawMode = false;
            return true;
        }
    }
}
