using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectre.Terminals.Ansi
{
    public sealed class EnableAlternativeBuffer : AnsiInstruction
    {
        public override void Accept<TState>(IAnsiSequenceVisitor<TState> visitor, TState state)
        {
            visitor.EnableAlternativeBuffer(this, state);
        }
    }
}
