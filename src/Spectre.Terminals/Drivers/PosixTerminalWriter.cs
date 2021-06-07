using System;
using System.Text;
using System.Threading;
using Mono.Unix.Native;

namespace Spectre.Terminals.Drivers
{
    internal sealed class PosixTerminalWriter : ITerminalWriter
    {
        private readonly int _handle;
        private readonly string _name;

        public Encoding Encoding
        {
            get => Encoding.UTF8;
            set { /* Do nothing for now */ }
        }

        public bool IsRedirected => !Syscall.isatty(_handle);

        public PosixTerminalWriter(int handle)
        {
            _handle = handle;
            _name = handle == PosixConstants.STDIN ? "STDIN" : "STDERR";
        }

        // From https://github.com/alexrp/system-terminal/blob/819090b722e3198b6b932fdd67641371be99e844/src/core/Drivers/UnixTerminalDriver.cs#L111
        public unsafe void Write(ReadOnlySpan<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                return;
            }

            var progress = 0;

            fixed (byte* p = buffer)
            {
                var len = buffer.Length;

                while (progress < len)
                {
                    long result;
                    while ((result = Syscall.write(_handle, p + progress, (ulong)(len - progress))) == -1 &&
                        Stdlib.GetLastError() == Errno.EINTR)
                    {
                    }

                    // The descriptor has been closed by someone else. Just silently ignore
                    // this situation.
                    if (result == 0)
                    {
                        break;
                    }

                    if (result != -1)
                    {
                        progress += (int)result;
                        continue;
                    }

                    var err = Stdlib.GetLastError();
                    if (err == Errno.EPIPE)
                    {
                        // The descriptor was probably redirected to a program that ended. Just
                        // silently ignore this situation.
                        break;
                    }
                    else if (err == Errno.EAGAIN)
                    {
                        // The file descriptor has been configured as non-blocking. Instead of
                        // busily trying to write over and over, poll until we can write and
                        // then try again.
                        Syscall.poll(
                            new[]
                            {
                                new Pollfd
                                {
                                    fd = _handle,
                                    events = PollEvents.POLLOUT,
                                },
                            }, 1, Timeout.Infinite);

                        continue;
                    }

                    throw new InvalidOperationException($"Could not write to standard {_name}: {Stdlib.strerror(err)}");
                }
            }
        }
    }
}
