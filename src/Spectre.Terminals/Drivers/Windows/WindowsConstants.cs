using System.Diagnostics.CodeAnalysis;

namespace Spectre.Terminals.Windows
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore")]
    internal static class WindowsConstants
    {
        public const int ERROR_HANDLE_EOF = 38;
        public const int ERROR_BROKEN_PIPE = 109;
        public const int ERROR_NO_DATA = 232;

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;

        public const int FILE_SHARE_READ = 1;
        public const int FILE_SHARE_WRITE = 2;

        public const uint CONSOLE_TEXTMODE_BUFFER = 1;

        public static class Colors
        {
            public const short Black = 0;
            public const short ForegroundBlue = 0x1;
            public const short ForegroundGreen = 0x2;
            public const short ForegroundRed = 0x4;
            public const short ForegroundYellow = 0x6;
            public const short ForegroundIntensity = 0x8;
            public const short BackgroundBlue = 0x10;
            public const short BackgroundGreen = 0x20;
            public const short BackgroundRed = 0x40;
            public const short BackgroundYellow = 0x60;
            public const short BackgroundIntensity = 0x80;
            public const short ForegroundMask = 0xf;
            public const short BackgroundMask = 0xf0;
            public const short ColorMask = 0xff;
        }
    }
}
