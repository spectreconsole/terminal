using System;
using System.Runtime.InteropServices;

namespace Spectre.Terminal
{
    public sealed class Terminal : ITerminal
    {
        private static readonly Lazy<ITerminal> _instance = new Lazy<ITerminal>(() => TerminalFactory.Create());

        public static ITerminal Shared => _instance.Value;

        private readonly ITerminalDriver _driver;
        private readonly object _lock;
        private bool _isRawMode;

        public TerminalInput Input { get; }
        public TerminalOutput Output { get; }
        public TerminalOutput Error { get; }

        public Terminal(ITerminalDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _lock = new object();

            Input = new TerminalInput(_driver.Input);
            Output = new TerminalOutput(_driver.Output);
            Error = new TerminalOutput(_driver.Error);
        }

        ~Terminal()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            DisableRawMode();
            _driver.Dispose();
        }

        public bool EnableRawMode()
        {
            lock (_lock)
            {
                if (_isRawMode)
                {
                    return true;
                }

                _isRawMode = _driver.EnableRawMode();
                return _isRawMode;
            }
        }

        public bool DisableRawMode()
        {
            lock (_lock)
            {
                if (!_isRawMode)
                {
                    return true;
                }

                _isRawMode = _driver.DisableRawMode();
                return !_isRawMode;
            }
        }
    }
}
