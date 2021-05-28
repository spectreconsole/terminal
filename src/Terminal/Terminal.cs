using System;

namespace Spectre.Terminal
{
    public sealed class Terminal : ITerminal
    {
        private static readonly Lazy<ITerminal> _instance;
        public static ITerminal Shared => _instance.Value;

        private readonly ITerminalDriver _driver;
        private readonly object _lock;

        public string Name => _driver.Name;
        public bool IsRawMode { get; private set; }

        public ITerminalReader Input { get; }
        public ITerminalWriter Output { get; }
        public ITerminalWriter Error { get; }

        static Terminal()
        {
            _instance = new Lazy<ITerminal>(() => TerminalFactory.Create());
        }

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
                if (IsRawMode)
                {
                    return true;
                }

                IsRawMode = _driver.EnableRawMode();
                return IsRawMode;
            }
        }

        public bool DisableRawMode()
        {
            lock (_lock)
            {
                if (!IsRawMode)
                {
                    return true;
                }

                IsRawMode = _driver.DisableRawMode();
                return !IsRawMode;
            }
        }
    }
}
