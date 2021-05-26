using System;
using Microsoft.Windows.Sdk;

namespace Spectre.Terminal
{
    internal sealed class WindowsDriver : ITerminalDriver
    {
        private const CONSOLE_MODE IN_MODE = CONSOLE_MODE.ENABLE_PROCESSED_INPUT | CONSOLE_MODE.ENABLE_LINE_INPUT | CONSOLE_MODE.ENABLE_ECHO_INPUT;
        private const CONSOLE_MODE OUT_MODE = CONSOLE_MODE.DISABLE_NEWLINE_AUTO_RETURN;

        public bool SupportsAnsi { get; }

        public WindowsTerminalReader Input { get; }
        public WindowsTerminalWriter Output { get; }
        public WindowsTerminalWriter Error { get; }

        ITerminalReader ITerminalDriver.Input => Input;
        ITerminalWriter ITerminalDriver.Output => Output;
        ITerminalWriter ITerminalDriver.Error => Error;

        public WindowsDriver()
        {
            Input = new WindowsTerminalReader();
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

            if (Output.GetMode(out var mode))
            {
                // Does STDOUT support ANSI?
                SupportsAnsi = (mode & CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0;
            }
            else if (Error.GetMode(out mode))
            {
                // Does STDERR support ANSI?
                SupportsAnsi = (mode & CONSOLE_MODE.ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0;
            }
        }

        public bool EnableRawMode()
        {
            if (!(Input.RemoveMode(IN_MODE) && (Output.RemoveMode(OUT_MODE) || Error.RemoveMode(OUT_MODE))))
            {
                return false;
            }

            if (!PInvoke.FlushConsoleInputBuffer(Input.Handle))
            {
                throw new InvalidOperationException("Could not flush input buffer");
            }

            return true;
        }

        public bool DisableRawMode()
        {
            if (!(Input.AddMode(IN_MODE) && (Output.AddMode(OUT_MODE) || Error.AddMode(OUT_MODE))))
            {
                return false;
            }

            if (!PInvoke.FlushConsoleInputBuffer(Input.Handle))
            {
                throw new InvalidOperationException("Could not flush input buffer");
            }

            return true;
        }
    }
}
