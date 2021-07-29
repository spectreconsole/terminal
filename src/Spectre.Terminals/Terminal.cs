using System;

namespace Spectre.Terminals
{
    /// <summary>
    /// Represents a terminal.
    /// </summary>
    public sealed class Terminal : ITerminal
    {
        private static readonly Lazy<ITerminal> _instance;

        /// <summary>
        /// Gets a lazily constructed, shared <see cref="ITerminal"/> instance.
        /// </summary>
        public static ITerminal Shared => _instance.Value;

        private readonly ITerminalDriver _driver;
        private readonly object _lock;

        /// <inheritdoc/>
        public string Name => _driver.Name;

        /// <inheritdoc/>
        public bool IsRawMode { get; private set; }

        /// <inheritdoc/>
        public event EventHandler<TerminalSignalEventArgs>? Signalled
        {
            add => _driver.Signalled += value;
            remove => _driver.Signalled += value;
        }

        /// <inheritdoc/>
        public TerminalSize? Size => _driver.Size;

        /// <inheritdoc/>
        public ITerminalInput Input { get; }

        /// <inheritdoc/>
        public ITerminalOutput Output { get; }

        /// <inheritdoc/>
        public ITerminalOutput Error { get; }

        static Terminal()
        {
            _instance = new Lazy<ITerminal>(() => TerminalFactory.Create());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Terminal"/> class.
        /// </summary>
        /// <param name="driver">The terminal driver.</param>
        public Terminal(ITerminalDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _lock = new object();

            Input = new TerminalInput(_driver.Input);
            Output = new TerminalOutput(_driver.Output);
            Error = new TerminalOutput(_driver.Error);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Terminal"/> class.
        /// </summary>
        ~Terminal()
        {
            Dispose();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            DisableRawMode();
            _driver.Dispose();
        }

        /// <inheritdoc/>
        public bool EmitSignal(TerminalSignal signal)
        {
            return _driver.EmitSignal(signal);
        }

        /// <inheritdoc/>
        public bool EnableRawMode()
        {
            lock (_lock)
            {
                if (IsRawMode)
                {
                    return true;
                }

                IsRawMode = _driver.EnableRawMode();
                return IsRawMode;
            }
        }

        /// <inheritdoc/>
        public bool DisableRawMode()
        {
            lock (_lock)
            {
                if (!IsRawMode)
                {
                    return true;
                }

                IsRawMode = !_driver.DisableRawMode();
                return !IsRawMode;
            }
        }
    }
}
