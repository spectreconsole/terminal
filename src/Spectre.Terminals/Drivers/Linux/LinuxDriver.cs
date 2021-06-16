using System;
using Mono.Unix.Native;
using static Spectre.Terminals.Drivers.LinuxInterop;

namespace Spectre.Terminals.Drivers
{
    internal sealed class LinuxDriver : UnixDriver
    {
        private termios? _original;
        private termios? _current;

        public override string Name { get; } = "Linux";

        public LinuxDriver()
        {
            if (tcgetattr(UnixConstants.STDIN, out var settings) == 0)
            {
                // These values are usually the default, but we set them just to be safe.
                settings.c_cc[VTIME] = 0;
                settings.c_cc[VMIN] = 1;

                _original = settings;

                // We might get really unlucky and fail to apply the settings right after the call
                // above. We should still assign _current so we can apply it later.
                if (!UpdateSettings(TCSANOW, settings))
                {
                    _current = settings;
                }
            }
        }

        public override bool EnableRawMode()
        {
            return SetRawMode(true);
        }

        public override bool DisableRawMode()
        {
            return SetRawMode(false);
        }

        public override void RefreshSettings()
        {
            if (_current is termios settings)
            {
                // This call can fail if the terminal is detached, but that is OK.
                UpdateSettings(TCSANOW, settings);
            }
        }

        public override TerminalSize? GetTerminalSize()
        {
            var result = ioctl(UnixConstants.STDOUT, (UIntPtr)TIOCGWINSZ, out var w);
            if (result == 0)
            {
                return new TerminalSize(w.ws_col, w.ws_row);
            }

            return null;
        }

        private bool SetRawMode(bool raw)
        {
            if (_original is not termios settings)
            {
                return false;
            }

            if (raw)
            {
                settings.c_iflag &= ~(IGNBRK | BRKINT | PARMRK | ISTRIP | INLCR | IGNCR | ICRNL | IXON);
                settings.c_oflag &= ~OPOST;
                settings.c_cflag &= ~(CSIZE | PARENB);
                settings.c_cflag |= CS8;
                settings.c_lflag &= ~(ISIG | ICANON | ECHO | ECHONL | IEXTEN);
            }

            return UpdateSettings(TCSAFLUSH, settings) ? true :
                throw new InvalidOperationException(
                    $"Could not change raw mode setting: {Stdlib.strerror(Stdlib.GetLastError())}");
        }

        private bool UpdateSettings(int mode, termios settings)
        {
            int result;
            while ((result = tcsetattr(UnixConstants.STDIN, mode, settings)) == -1
                && Stdlib.GetLastError() == Errno.EINTR)
            {
                // Retry in case we get interrupted by a signal.
            }

            if (result == 0)
            {
                _current = settings;
                return true;
            }

            return false;
        }
    }
}
