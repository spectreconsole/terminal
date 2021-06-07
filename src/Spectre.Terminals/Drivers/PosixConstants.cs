using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mono.Unix.Native;

namespace Spectre.Terminals.Drivers
{
    internal sealed class PosixConstants
    {
        public const int STDIN = 0;
        public const int STDOUT = 1;
        public const int STDERR = 2;
    }
}
