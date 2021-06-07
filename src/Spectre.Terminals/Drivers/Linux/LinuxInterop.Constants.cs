namespace Spectre.Terminals.Drivers
{
    internal sealed partial class LinuxInterop
    {
        /// <summary>
        /// winsize: Fill in the winsize structure pointed to by the
        /// third argument with the screen width and height.
        /// </summary>
        public const uint TIOCGWINSZ = 0x5413;

        /// <summary>
        /// termio: Apply changes immediately.
        /// </summary>
        public const int TCSANOW = 0;

        /// <summary>
        /// termio: the change occurs after all output written to the object
        /// referred by fd has been transmitted, and all input that
        /// has been received but not read will be discarded before
        /// the change is made.
        /// </summary>
        public const int TCSAFLUSH = 2;

        /// <summary>
        /// c_iflag: Ignore BREAK condition on input.
        /// </summary>
        public const uint IGNBRK = 0x1;

        /// <summary>
        /// c_iflag: If IGNBRK is set, a BREAK is ignored.  If it is not set
        /// but BRKINT is set, then a BREAK causes the input and
        /// output queues to be flushed, and if the terminal is the
        /// controlling terminal of a foreground process group, it
        /// will cause a SIGINT to be sent to this foreground process
        /// group.When neither IGNBRK nor BRKINT are set, a BREAK
        /// reads as a null byte <c>('\0')</c>, except when PARMRK is set, in
        /// which case it reads as the sequence <c>\377 \0 \0</c>.
        /// </summary>
        public const uint BRKINT = 0x2;

        /// <summary>
        /// c_iflag: If this bit is set, input bytes with parity or framing
        /// errors are marked when passed to the program.  This bit is
        /// meaningful only when INPCK is set and IGNPAR is not set.
        /// The way erroneous bytes are marked is with two preceding
        /// bytes, \377 and \0.  Thus, the program actually reads
        /// three bytes for one erroneous byte received from the
        /// terminal.  If a valid byte has the value \377, and ISTRIP
        /// (see below) is not set, the program might confuse it with
        /// the prefix that marks a parity error.  Therefore, a valid
        /// byte \377 is passed to the program as two bytes, \377
        /// \377, in this case.
        ///
        /// If neither IGNPAR nor PARMRK is set, read a character with
        /// a parity error or framing error as \0.
        /// </summary>
        public const uint PARMRK = 0x8;

        /// <summary>
        /// c_iflag: Strip off eighth bit.
        /// </summary>
        public const uint ISTRIP = 0x20;

        /// <summary>
        /// c_iflag: Translate NL to CR on input.
        /// </summary>
        public const uint INLCR = 0x40;

        /// <summary>
        /// c_iflag: Ignore carriage return on input.
        /// </summary>
        public const uint IGNCR = 0x80;

        /// <summary>
        /// c_iflag: Translate carriage return to newline on input (unless IGNCR is set).
        /// </summary>
        public const uint ICRNL = 0x100;

        /// <summary>
        /// c_iflag: (not in POSIX) Map uppercase characters to lowercase on input.
        /// </summary>
        public const uint IXON = 0x400;

        /// <summary>
        /// c_oflag: Enable implementation-defined output processing.
        /// </summary>
        public const uint OPOST = 0x1;

        /// <summary>
        /// c_cflag: Character size mask. Values are CS5, CS6, CS7, or CS8.
        /// </summary>
        public const uint CSIZE = 0x30;

        /// <summary>
        /// c_cflag: Character size mask: 8 bit.
        /// </summary>
        public const uint CS8 = 0x30;

        /// <summary>
        /// c_cflag: Enable parity generation on output and parity checking for input.
        /// </summary>
        public const uint PARENB = 0x100;

        /// <summary>
        /// c_lflag: When any of the characters INTR, QUIT, SUSP, or DSUSP are
        /// received, generate the corresponding signal.
        /// </summary>
        public const uint ISIG = 0x1;

        /// <summary>
        /// c_lflag: Enable canonical mode.
        /// Input is made available line by line.
        /// An input line is available when one of the line delimiters is typed
        /// (NL, EOL, EOL2; or EOF at the start of line).
        /// Line editing is enabled.
        /// The maximum line length is 4096 chars (including the terminating newline character).
        /// </summary>
        public const uint ICANON = 0x2;

        /// <summary>
        /// c_lflag: Echo input characters.
        /// </summary>
        public const uint ECHO = 0x8;

        /// <summary>
        /// c_lflag: If ICANON is also set, echo the NL character even if ECHO is not set.
        /// </summary>
        public const uint ECHONL = 0x40;

        /// <summary>
        /// c_lflag: Enable implementation-defined input processing.
        /// This flag, as well as ICANON must be enabled for the special
        /// characters EOL2, LNEXT, REPRINT, WERASE to be interpreted,
        /// and for the IUCLC flag to be effective.
        /// </summary>
        public const uint IEXTEN = 0x8000;

        /// <summary>
        /// c_cc: Timeout in deciseconds for noncanonical read (TIME).
        /// </summary>
        public const int VTIME = 5;

        /// <summary>
        /// c_cc: Minimum number of characters for noncanonical read (MIN).
        /// </summary>
        public const int VMIN = 6;
    }
}
