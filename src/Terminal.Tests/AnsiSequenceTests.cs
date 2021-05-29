using System.Text;
using Shouldly;
using Spectre.Terminal.Ansi;
using Xunit;

namespace Spectre.Terminal.Tests
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
                    "\u001b[2KHello \u001b[2BWorld!\u001b[1G!");

                // Then
                state.ToString()
                    .ShouldBe("[EL2]Hello [CUD2]World![CHA1]!");
            }

            private sealed class AnsiPrinter : AnsiSequenceVisitor<StringBuilder>
            {
                protected override void CursorDown(CursorDown instruction, StringBuilder context)
                {
                    context.Append($"[CUD{instruction.Count}]");
                }

                protected override void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, StringBuilder context)
                {
                    context.Append($"[CHA{instruction.Column}]");
                }

                protected override void EraseInLine(EraseInLine instruction, StringBuilder context)
                {
                    context.Append($"[EL{instruction.Mode}]");
                }

                protected override void PrintText(PrintText instruction, StringBuilder context)
                {
                    context.Append(instruction.Text.ToString());
                }
            }
        }
    }
}
