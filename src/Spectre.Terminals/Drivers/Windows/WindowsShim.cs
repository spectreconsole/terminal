// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#if !Windows
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Windows.Sdk
{
    internal enum STD_HANDLE_TYPE : uint
    {
        STD_INPUT_HANDLE = 4294967286U,
        STD_OUTPUT_HANDLE = 4294967285U,
        STD_ERROR_HANDLE = 4294967284U,
    }

    [Flags]
    internal enum CONSOLE_MODE : uint
    {
        ENABLE_ECHO_INPUT = 0x00000004,
        ENABLE_INSERT_MODE = 0x00000020,
        ENABLE_LINE_INPUT = 0x00000002,
        ENABLE_MOUSE_INPUT = 0x00000010,
        ENABLE_PROCESSED_INPUT = 0x00000001,
        ENABLE_QUICK_EDIT_MODE = 0x00000040,
        ENABLE_WINDOW_INPUT = 0x00000008,
        ENABLE_VIRTUAL_TERMINAL_INPUT = 0x00000200,
        ENABLE_PROCESSED_OUTPUT = 0x00000001,
        ENABLE_WRAP_AT_EOL_OUTPUT = 0x00000002,
        ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x00000004,
        DISABLE_NEWLINE_AUTO_RETURN = 0x00000008,
        ENABLE_LVB_GRID_WORLDWIDE = 0x00000010,
    }

    internal partial struct SMALL_RECT
    {
        internal short Left;
        internal short Top;
        internal short Right;
        internal short Bottom;
    }

    internal partial struct CONSOLE_SCREEN_BUFFER_INFO
    {
        internal COORD dwSize;
        internal COORD dwCursorPosition;
        internal ushort wAttributes;
        internal SMALL_RECT srWindow;
        internal COORD dwMaximumWindowSize;
    }

    internal partial struct CONSOLE_SCREEN_BUFFER_INFOEX
    {
        internal uint cbSize;
        internal COORD dwSize;
        internal COORD dwCursorPosition;
        internal ushort wAttributes;
        internal SMALL_RECT srWindow;
        internal COORD dwMaximumWindowSize;
        internal ushort wPopupAttributes;
        internal BOOL bFullscreenSupported;
        internal __uint_16 ColorTable;
        internal struct __uint_16
        {
            internal uint _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15;
            /// <summary>Always <c>16</c>.</summary>
            internal int Length => 16;
            /// <summary>
            /// Gets a ref to an individual element of the inline array.
            /// ⚠ Important ⚠: When this struct is on the stack, do not let the returned reference outlive the stack frame that defines it.
            /// </summary>
            internal ref uint this[int index] => ref AsSpan()[index];
            /// <summary>
            /// Gets this inline array as a span.
            /// </summary>
            /// <remarks>
            /// ⚠ Important ⚠: When this struct is on the stack, do not let the returned span outlive the stack frame that defines it.
            /// </remarks>
            internal Span<uint> AsSpan() => MemoryMarshal.CreateSpan(ref _0, 16);
        }
    }

    internal partial struct OVERLAPPED
    {
    }

    internal readonly partial struct HANDLE
    {
        internal readonly IntPtr Value;
        internal HANDLE(IntPtr value) => this.Value = value;
    }

    internal partial struct COORD
    {
        internal short X;
        internal short Y;
    }

    internal readonly partial struct BOOL
    {
        private readonly int value;
        internal int Value => this.value;
        internal BOOL(bool value) => this.value = value ? 1 : 0;
        internal BOOL(int value) => this.value = value;
        public static implicit operator bool (BOOL value) => value.Value != 0;
        public static implicit operator BOOL(bool value) => new BOOL(value);
        public static explicit operator BOOL(int value) => new BOOL(value);
    }

    internal partial struct SECURITY_ATTRIBUTES
    {
        internal uint nLength;
        internal unsafe void* lpSecurityDescriptor;
        internal BOOL bInheritHandle;
    }

    internal partial struct CONSOLE_CURSOR_INFO
    {
        internal uint dwSize;
        internal BOOL bVisible;
    }

    internal unsafe delegate BOOL PHANDLER_ROUTINE(uint CtrlType);

    internal static partial class PInvoke
    {
        internal static bool SetConsoleTextAttribute(HANDLE hConsoleOutput, ushort wAttributes)
        {
            throw new PlatformNotSupportedException();
        }

        internal static bool SetConsoleCtrlHandler(PHANDLER_ROUTINE HandlerRoutine, bool Add)
        {
            throw new PlatformNotSupportedException();
        }

        internal static bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId)
        {
            throw new PlatformNotSupportedException();
        }

        internal static bool SetConsoleCP(uint wCodePageID)
        {
            throw new PlatformNotSupportedException();
        }

        internal static uint GetConsoleCP()
        {
            throw new PlatformNotSupportedException();
        }

        internal static uint GetConsoleOutputCP()
        {
            throw new PlatformNotSupportedException();
        }

        internal static bool SetConsoleOutputCP(uint wCodePageID)
        {
            throw new PlatformNotSupportedException();
        }

        internal static BOOL CloseHandle(HANDLE hObject)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe Microsoft.Win32.SafeHandles.SafeFileHandle GetStdHandle_SafeHandle(STD_HANDLE_TYPE nStdHandle)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool WriteFile(SafeHandle hFile, void* lpBuffer, uint nNumberOfBytesToWrite, uint* lpNumberOfBytesWritten, OVERLAPPED* lpOverlapped)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool ReadFile(SafeHandle hFile, void* lpBuffer, uint nNumberOfBytesToRead, uint* lpNumberOfBytesRead, OVERLAPPED* lpOverlapped)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool SetConsoleMode(SafeHandle hConsoleHandle, CONSOLE_MODE dwMode)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool GetConsoleMode(SafeHandle hConsoleHandle, out CONSOLE_MODE lpMode)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe uint GetFileType(SafeHandle hFile)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool FlushConsoleInputBuffer(SafeHandle hConsoleInput)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool GetConsoleScreenBufferInfo(SafeHandle hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool GetConsoleScreenBufferInfoEx(SafeHandle hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFOEX lpConsoleScreenBufferInfoEx)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool SetConsoleCursorPosition(SafeHandle hConsoleOutput, COORD dwCursorPosition)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool FillConsoleOutputCharacter(SafeHandle hConsoleOutput, ushort cCharacter, uint nLength, COORD dwWriteCoord, out uint lpNumberOfCharsWritten)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool GetConsoleCursorInfo(SafeHandle hConsoleOutput, out CONSOLE_CURSOR_INFO lpConsoleCursorInfo)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool SetConsoleCursorInfo(SafeHandle hConsoleOutput, in CONSOLE_CURSOR_INFO lpConsoleCursorInfo)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe Microsoft.Win32.SafeHandles.SafeFileHandle CreateConsoleScreenBuffer(uint dwDesiredAccess, uint dwShareMode, SECURITY_ATTRIBUTES? lpSecurityAttributes, uint dwFlags, void* lpScreenBufferData)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool SetConsoleActiveScreenBuffer(SafeHandle hConsoleOutput)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe COORD GetLargestConsoleWindowSize(SafeHandle hConsoleOutput)
        {
            throw new PlatformNotSupportedException();
        }

        internal static unsafe bool SetConsoleTextAttribute(SafeHandle hConsoleOutput, ushort wAttributes)
        {
            throw new PlatformNotSupportedException();
        }
    }
}
#endif