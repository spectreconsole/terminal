using System;
using System.Collections.Generic;

namespace Spectre.Terminal.Ansi
{
    internal sealed class AnsiInstructionToken
    {
        private readonly ReadOnlyMemory<char>? _span;
        private readonly IReadOnlyList<AnsiSequenceToken>? _tokens;

        public ReadOnlyMemory<char> Span => _span ?? ReadOnlyMemory<char>.Empty;
        public IReadOnlyList<AnsiSequenceToken> Tokens => _tokens ?? Array.Empty<AnsiSequenceToken>();

        public bool IsText => _span != null;
        public bool IsAnsiEscapeSequence => _tokens != null;

        private AnsiInstructionToken(ReadOnlyMemory<char>? span, IReadOnlyList<AnsiSequenceToken>? tokens)
        {
            _span = span;
            _tokens = tokens;
        }

        public static AnsiInstructionToken Text(ReadOnlyMemory<char> span)
        {
            return new AnsiInstructionToken(span, null);
        }

        public static AnsiInstructionToken Sequence(IReadOnlyList<AnsiSequenceToken>? tokens)
        {
            return new AnsiInstructionToken(null, tokens);
        }
    }
}
