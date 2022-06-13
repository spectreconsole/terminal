namespace Spectre.Terminals.Drivers;

internal sealed class WindowsConsoleStream : Stream
{
    private const int BytesPerWChar = 2;

    private readonly SafeHandle _handle;
    private readonly bool _useFileAPIs;

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => throw new NotSupportedException();
    public override long Position
    {
        get { throw new NotSupportedException(); }
        set { throw new NotSupportedException(); }
    }

    public WindowsConsoleStream(SafeHandle handle, bool useFileApis)
    {
        _handle = handle ?? throw new ArgumentNullException(nameof(handle));
        _useFileAPIs = useFileApis;
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return ReadFromHandle(new Span<byte>(buffer, offset, count));
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    private unsafe int ReadFromHandle(Span<byte> buffer)
    {
        if (buffer.IsEmpty)
        {
            return 0;
        }

        bool readSuccess;
        var bytesRead = 0;

        fixed (byte* p = buffer)
        {
            if (_useFileAPIs)
            {
                uint result;
                var ptrResult = &result;
                readSuccess = PInvoke.ReadFile(_handle, p, (uint)buffer.Length, ptrResult, null);
                bytesRead = (int)result;
            }
            else
            {
                uint result;
                var ptrResult = &result;
                readSuccess = PInvoke.ReadConsole(_handle, p, (uint)buffer.Length, out var charsRead, null);
                bytesRead = (int)(charsRead * BytesPerWChar);
            }
        }

        if (readSuccess)
        {
            return bytesRead;
        }

        throw new InvalidOperationException("Could not read from console input");
    }
}
