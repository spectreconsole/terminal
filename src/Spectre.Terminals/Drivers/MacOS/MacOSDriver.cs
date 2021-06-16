using System;
using Mono.Unix.Native;
using static Spectre.Terminals.Drivers.MacOSInterop;

namespace Spectre.Terminals.Drivers
{
    internal sealed class MacOSDriver : PosixDriver
    {
        private termios? _original;
        private termios? _current;

        public override string Name => "macOS";

        public MacOSDriver()
        {
            if (tcgetattr(PosixConstants.STDIN, out var settings) == 0)
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

        private bool SetRawMode(bool raw)
        {
            if (_original is not termios settings)
            {
                return false;
            }

            if (raw)
            {
                var iflag = (uint)settings.c_iflag;
                var oflag = (uint)settings.c_oflag;
                var cflag = (uint)settings.c_cflag;
                var lflag = (uint)settings.c_lflag;

                iflag &= ~(IGNBRK | BRKINT | PARMRK | ISTRIP | INLCR | IGNCR | ICRNL | IXON);
                oflag &= ~OPOST;
                cflag &= ~(CSIZE | PARENB);
                cflag |= CS8;
                lflag &= ~(ISIG | ICANON | ECHO | ECHONL | IEXTEN);

                settings.c_iflag = (UIntPtr)iflag;
                settings.c_oflag = (UIntPtr)oflag;
                settings.c_cflag = (UIntPtr)cflag;
                settings.c_lflag = (UIntPtr)lflag;
            }

            return UpdateSettings(TCSAFLUSH, settings) ? true :
                throw new InvalidOperationException(
                    $"Could not change raw mode setting: {Stdlib.strerror(Stdlib.GetLastError())}");
        }

        public override TerminalSize? GetTerminalSize()
        {
            var result = ioctl(PosixConstants.STDOUT, (UIntPtr)TIOCGWINSZ, out var w);
            if (result == 0)
            {
                return new TerminalSize(w.ws_col, w.ws_row);
            }

            return null;
        }

        public override void RefreshSettings()
        {
            if (_current is termios settings)
            {
                // This call can fail if the terminal is detached, but that is OK.
                UpdateSettings(TCSANOW, settings);
            }
        }

        private bool UpdateSettings(int mode, termios settings)
        {
            int result;
            while ((result = tcsetattr(PosixConstants.STDIN, mode, settings)) == -1
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
