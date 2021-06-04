using System.Threading;
using Spectre.Terminals;

namespace Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var terminal = Terminal.Shared;
            var cancel = new ManualResetEvent(false);

            // Hook up signal handling
            terminal.Signalled += (s, e) =>
            {
                if (e.Signal == TerminalSignal.SIGINT)
                {
                    terminal.WriteLine("Received \u001b[38;5;14mSIGINT\u001b[0m");
                    e.Cancel = true;
                    cancel.Set();
                }
                else if(e.Signal == TerminalSignal.SIGQUIT)
                {
                    terminal.WriteLine("Received \u001b[38;5;14mSIGQUIT\u001b[0m");
                    e.Cancel = true;
                    cancel.Set();
                }
            };

            // Wait for a signal
            terminal.WriteLine("Press CTRL+C or CTRL+BREAK to quit");
            cancel.WaitOne();
            terminal.WriteLine("Bye!");
        }
    }
}
