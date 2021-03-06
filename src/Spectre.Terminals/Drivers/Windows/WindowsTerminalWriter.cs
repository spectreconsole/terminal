namespace Spectre.Terminals.Drivers;

internal sealed class WindowsTerminalWriter : WindowsTerminalHandle, IWindowsTerminalWriter
{
    private readonly string _name;
    private Encoding _encoding;

    public Encoding Encoding
    {
        get => _encoding;
        set => SetEncoding(value);
    }

    public WindowsTerminalWriter(STD_HANDLE_TYPE handle)
        : base(handle)
    {
        _name = handle == STD_HANDLE_TYPE.STD_OUTPUT_HANDLE ? "STDOUT" : "STDERR";
        _encoding = EncodingHelper.GetEncodingFromCodePage((int)PInvoke.GetConsoleOutputCP());
    }

    public unsafe void Write(ReadOnlySpan<byte> buffer)
    {
        Write(Handle, buffer);
    }

    public unsafe void Write(SafeHandle handle, ReadOnlySpan<byte> buffer)
    {
        if (buffer.IsEmpty)
        {
            return;
        }

        uint written;
        uint* ptrWritten = &written;

        fixed (byte* ptrData = buffer)
        {
            if (PInvoke.WriteFile(handle, ptrData, (uint)buffer.Length, ptrWritten, null))
            {
                return;
            }
        }

        switch (Marshal.GetLastWin32Error())
        {
            case WindowsConstants.ERROR_HANDLE_EOF:
            case WindowsConstants.ERROR_BROKEN_PIPE:
            case WindowsConstants.ERROR_NO_DATA:
                break;
            default:
                throw new InvalidOperationException($"Could not write to {_name}");
        }
    }

    private void SetEncoding(Encoding encoding)
    {
        if (encoding is null)
        {
            throw new ArgumentNullException(nameof(encoding));
        }

        if (PInvoke.SetConsoleOutputCP((uint)encoding.CodePage))
        {
            _encoding = encoding;
        }
    }
}
