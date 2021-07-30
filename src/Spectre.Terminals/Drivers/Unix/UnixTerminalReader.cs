using System;
using System.Text;
using System.Threading;
using Mono.Unix.Native;

namespace Spectre.Terminals.Drivers
{
    internal sealed class UnixTerminalReader : ITerminalReader
    {
        private readonly Encoding _encoding;

        public Encoding Encoding
        {
            get => _encoding;
            set { /* Do nothing for now */ }
        }

        public bool IsKeyAvailable => throw new NotSupportedException("Not yet supported");

        public bool IsRedirected => !Syscall.isatty(UnixConstants.STDIN);

        public UnixTerminalReader()
        {
            _encoding = EncodingHelper.GetEncodingFromCharset() ?? new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        }

        public int Read()
        {
            throw new NotSupportedException("Not yet supported");
        }

        public string? ReadLine()
        {
            throw new NotSupportedException("Not yet supported");
        }

        public ConsoleKeyInfo ReadKey()
        {
            throw new NotSupportedException("Not yet supported");
        }

        private static unsafe int Read(Span<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                return 0;
            }

            long ret;

            while (true)
            {
                fixed (byte* p = buffer)
                {
                    while ((ret = Syscall.read(UnixConstants.STDIN, p, (ulong)buffer.Length)) == -1 &&
                        Stdlib.GetLastError() == Errno.EINTR)
                    {
                        // Retry in case we get interrupted by a signal.
                    }

                    if (ret != -1)
                    {
                        break;
                    }

                    var err = Stdlib.GetLastError();

                    // The descriptor was probably redirected to a program that ended. Just
                    // silently ignore this situation.
                    //
                    // The strange condition where errno is zero happens e.g. on Linux if
                    // the process is killed while blocking in the read system call.
                    if (err == 0 || err == Errno.EPIPE)
                    {
                        ret = 0;

                        break;
                    }

                    // The file descriptor has been configured as non-blocking. Instead of
                    // busily trying to read over and over, poll until we can write and then
                    // try again.
                    if (err == Errno.EAGAIN)
                    {
                        _ = Syscall.poll(
                            new[]
                            {
                                new Pollfd
                                {
                                    fd = UnixConstants.STDIN,
                                    events = PollEvents.POLLIN,
                                },
                            }, 1, Timeout.Infinite);

                        continue;
                    }

                    if (err == 0)
                    {
                        err = Errno.EBADF;
                    }

                    throw new InvalidOperationException(
                        $"Could not read from STDIN: {Stdlib.strerror(err)}");
                }
            }

            return (int)ret;
        }
    }
}
