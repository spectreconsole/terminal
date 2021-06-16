using System;
using System.Collections.Generic;

namespace Spectre.Terminals.Emulation
{
    internal sealed class AnsiToken
    {
        private readonly ReadOnlyMemory<char>? _span;
        private readonly IReadOnlyList<AnsiSequenceToken>? _tokens;

        public ReadOnlyMemory<char> Text => _span ?? ReadOnlyMemory<char>.Empty;
        public IReadOnlyList<AnsiSequenceToken> Sequence => _tokens ?? Array.Empty<AnsiSequenceToken>();

        public bool IsText => _span != null;
        public bool IsSequence => _tokens != null;

        private AnsiToken(ReadOnlyMemory<char>? span, IReadOnlyList<AnsiSequenceToken>? tokens)
        {
            _span = span;
            _tokens = tokens;
        }

        public static AnsiToken CreateText(ReadOnlyMemory<char> span)
        {
            return new AnsiToken(span, null);
        }

        public static AnsiToken CreateSequence(IReadOnlyList<AnsiSequenceToken>? tokens)
        {
            return new AnsiToken(null, tokens);
        }
    }
}
