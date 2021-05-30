using System.Text;
using Shouldly;
using Spectre.Terminals.Windows.Emulation;
using Xunit;

namespace Spectre.Terminals.Tests
{
    public sealed class AnsiSequenceTests
    {
        public sealed class TheInterpretMethod
        {
            [Fact]
            public void Should_Run_Sequence()
            {
                // Given
                var printer = new AnsiPrinter();
                var state = new StringBuilder();

                // When
                AnsiInterpreter.Interpret(
                    printer, state,
                    "\u001b[?25l\u001b[2KHello \u001b[2BWorld!\u001b[1G!\u001b[?25h");

                // Then
                state.ToString()
                    .ShouldBe("[HideCursor][EL2]Hello [CUD2]World![CHA1]![ShowCursor]");
            }

            private sealed class AnsiPrinter : AnsiSequenceVisitor<StringBuilder>
            {
                protected override void CursorDown(CursorDown instruction, StringBuilder state)
                {
                    state.Append($"[CUD{instruction.Count}]");
                }

                protected override void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, StringBuilder state)
                {
                    state.Append($"[CHA{instruction.Column}]");
                }

                protected override void EraseInLine(EraseInLine instruction, StringBuilder state)
                {
                    state.Append($"[EL{instruction.Mode}]");
                }

                protected override void PrintText(PrintText instruction, StringBuilder state)
                {
                    state.Append(instruction.Text.ToString());
                }

                protected override void ShowCursor(ShowCursor instruction, StringBuilder state)
                {
                    state.Append("[ShowCursor]");
                }

                protected override void HideCursor(HideCursor instruction, StringBuilder state)
                {
                    state.Append("[HideCursor]");
                }
            }
        }
    }
}
