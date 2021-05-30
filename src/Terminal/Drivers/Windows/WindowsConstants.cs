namespace Spectre.Terminal.Windows
{
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
    }
}
