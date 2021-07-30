using System.Diagnostics.CodeAnalysis;

namespace Spectre.Terminals.Drivers
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore")]
    internal static class WindowsConstants
    {
        public const int ERROR_HANDLE_EOF = 38;
        public const int ERROR_BROKEN_PIPE = 109;
        public const int ERROR_NO_DATA = 232;

        public const int KEY_EVENT = 0x0001;

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;

        public const int FILE_SHARE_READ = 1;
        public const int FILE_SHARE_WRITE = 2;

        public const uint CONSOLE_TEXTMODE_BUFFER = 1;

        public static class Signals
        {
            public const uint CTRL_C_EVENT = 0;
            public const uint CTRL_BREAK_EVENT = 1;
        }

        public static class Colors
        {
            public const short FOREGROUND_MASK = 0xf;
            public const short BACKGROUND_MASK = 0xf0;
            public const short COLOR_MASK = 0xff;
        }
    }
}
