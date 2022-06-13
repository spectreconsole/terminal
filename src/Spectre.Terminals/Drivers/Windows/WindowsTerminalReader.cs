namespace Spectre.Terminals.Drivers;

internal sealed class WindowsTerminalReader : WindowsTerminalHandle, ITerminalReader
{
    private readonly WindowsDriver _driver;
    private readonly WindowsKeyReader _keyReader;
    private readonly SynchronizedTextReader _reader;
    private Encoding _encoding;

    public Encoding Encoding
    {
        get => _encoding;
        set => SetEncoding(value);
    }

    public bool IsRawMode => _driver.IsRawMode;
    public bool IsKeyAvailable => _keyReader.IsKeyAvailable();

    public WindowsTerminalReader(WindowsDriver driver)
        : base(STD_HANDLE_TYPE.STD_INPUT_HANDLE)
    {
        _encoding = EncodingHelper.GetEncodingFromCodePage((int)PInvoke.GetConsoleCP());
        _driver = driver;
        _keyReader = new WindowsKeyReader(Handle);
        _reader = CreateReader(Handle, _encoding, IsRedirected);
    }

    public int Read()
    {
        return _reader.Read();
    }

    public string? ReadLine()
    {
        return _reader.ReadLine();
    }

    public ConsoleKeyInfo ReadKey()
    {
        return _keyReader.ReadKey();
    }

    private static SynchronizedTextReader CreateReader(SafeHandle handle, Encoding encoding, bool isRedirected)
    {
        static Stream Create(SafeHandle handle, bool useFileApis)
        {
            if (handle.IsInvalid || handle.IsClosed)
            {
                return Stream.Null;
            }
            else
            {
                return new WindowsConsoleStream(handle, useFileApis);
            }
        }

        var useFileApis = !encoding.IsUnicode() || isRedirected;

        var stream = Create(handle, useFileApis);
        if (stream == null || stream == Stream.Null)
        {
            return new SynchronizedTextReader(StreamReader.Null);
        }

        return new SynchronizedTextReader(
            new StreamReader(
                stream: stream,
                encoding: new EncodingWithoutPreamble(encoding),
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 4096,
                leaveOpen: true));
    }

    private void SetEncoding(Encoding encoding)
    {
        if (PInvoke.SetConsoleCP((uint)encoding.CodePage))
        {
            // TODO 2021-07-31: Recreate text reader
            _encoding = encoding;
        }
    }
}
