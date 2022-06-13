namespace Spectre.Terminals.Tests;

public sealed class AnsiSequenceTests
{
    public sealed class TheInterpretMethod
    {
        [Fact]
        public void Should_Interpret_Codes_As_Expected()
        {
            // Given
            var printer = new AnsiPrinter();
            var state = new StringBuilder();

            // When
            AnsiInterpreter.Interpret(
                printer, state,
                "\u001b[?1049h " +
                "\u001b[?25l\u001b[2KHello World!\u001b[?25h " +
                "\u001b[3A \u001b[4B \u001b[5C \u001b[6D " +
                "\u001b[7E \u001b[8F \u001b[9G \u001b[10;11H " +
                "\u001b[0J \u001b[1J \u001b[2J \u001b[3J " +
                "\u001b[0K \u001b[1K \u001b[2K " +
                "\u001b[s \u001b[u " +
                "\u001b[?1049l");

            // Then
            state.ToString()
                .ShouldBe(
                    "[EnableAltBuffer] " +
                    "[HideCursor][EL2]Hello World![ShowCursor] " +
                    "[CUU3] [CUD4] [CUF5] [CUB6] " +
                    "[CNL7] [CPL8] [CHA9] [CUP10,11] " +
                    "[ED0] [ED1] [ED2] [ED3] " +
                    "[EL0] [EL1] [EL2] " +
                    "[SaveCursor] [RestoreCursor] " +
                    "[DisableAltBuffer]");
        }

        [Theory]
        [InlineData("\u001b[35m", "[SGR-FG=5]")]
        [InlineData("\u001b[38;5;29m", "[SGR-FG=29]")]
        [InlineData("\u001b[38;2;92;128;255m", "[SGR-FG=92,128,255]")]
        [InlineData("\u001b[42m", "[SGR-BG=2]")]
        [InlineData("\u001b[48;5;29m", "[SGR-BG=29]")]
        [InlineData("\u001b[48;2;92;128;255m", "[SGR-BG=92,128,255]")]
        [InlineData("\u001b[0m", "[SGR-RESET]")]
        public void Should_Interpret_SGR_Attributes_Correctly(string input, string expected)
        {
            // Given
            var printer = new AnsiPrinter();
            var state = new StringBuilder();

            // When
            AnsiInterpreter.Interpret(printer, state, input);

            // Then
            state.ToString().ShouldBe(expected);
        }

        [Theory]
        [InlineData("\u001b[38;5m")]
        [InlineData("\u001b[38;2;92;128m")]
        [InlineData("\u001b[38;2;92m")]
        [InlineData("\u001b[38;2m")]
        [InlineData("\u001b[48;5m")]
        [InlineData("\u001b[48;2;92;128m")]
        [InlineData("\u001b[48;2;92m")]
        [InlineData("\u001b[48;2m")]
        public void Should_Not_Parse_Malformed_SGR_Attributes(string input)
        {
            // Given
            var printer = new AnsiPrinter();
            var state = new StringBuilder();

            // When
            AnsiInterpreter.Interpret(printer, state, input);

            // Then
            state.Length.ShouldBe(0);
        }

        private sealed class AnsiPrinter : AnsiSequenceVisitor<StringBuilder>
        {
            protected override void PrintText(PrintText instruction, StringBuilder state)
            {
                state.Append(instruction.Text);
            }

            protected override void ShowCursor(ShowCursor instruction, StringBuilder state)
            {
                state.Append("[ShowCursor]");
            }

            protected override void HideCursor(HideCursor instruction, StringBuilder state)
            {
                state.Append("[HideCursor]");
            }

            protected override void CursorUp(CursorUp instruction, StringBuilder state)
            {
                state.Append("[CUU").Append(instruction.Count).Append(']');
            }

            protected override void CursorDown(CursorDown instruction, StringBuilder state)
            {
                state.Append("[CUD").Append(instruction.Count).Append(']');
            }

            protected override void CursorBack(CursorBack instruction, StringBuilder state)
            {
                state.Append("[CUB").Append(instruction.Count).Append(']');
            }

            protected override void CursorForward(CursorForward instruction, StringBuilder state)
            {
                state.Append("[CUF").Append(instruction.Count).Append(']');
            }

            protected override void CursorHorizontalAbsolute(CursorHorizontalAbsolute instruction, StringBuilder state)
            {
                state.Append("[CHA").Append(instruction.Column).Append(']');
            }

            protected override void CursorNextLine(CursorNextLine instruction, StringBuilder state)
            {
                state.Append("[CNL").Append(instruction.Count).Append(']');
            }

            protected override void CursorPosition(CursorPosition instruction, StringBuilder state)
            {
                state.Append("[CUP").Append(instruction.Row).Append(',').Append(instruction.Column).Append(']');
            }

            protected override void CursorPreviousLine(CursorPreviousLine instruction, StringBuilder state)
            {
                state.Append("[CPL").Append(instruction.Count).Append(']');
            }

            protected override void DisableAlternativeBuffer(DisableAlternativeBuffer instruction, StringBuilder state)
            {
                state.Append("[DisableAltBuffer]");
            }

            protected override void EnableAlternativeBuffer(EnableAlternativeBuffer instruction, StringBuilder state)
            {
                state.Append("[EnableAltBuffer]");
            }

            protected override void EraseInDisplay(EraseInDisplay instruction, StringBuilder state)
            {
                state.Append("[ED").Append(instruction.Mode).Append(']');
            }

            protected override void EraseInLine(EraseInLine instruction, StringBuilder state)
            {
                state.Append("[EL").Append(instruction.Mode).Append(']');
            }

            protected override void RestoreCursor(RestoreCursor instruction, StringBuilder state)
            {
                state.Append("[RestoreCursor]");
            }

            protected override void StoreCursor(StoreCursor instruction, StringBuilder state)
            {
                state.Append("[SaveCursor]");
            }

            protected override void SelectGraphicRendition(SelectGraphicRendition instruction, StringBuilder state)
            {
                foreach (var operation in instruction.Operations)
                {
                    if (operation.Reset)
                    {
                        state.Append("[SGR-RESET]");
                    }
                    else if (operation.Foreground != null)
                    {
                        state.Append($"[SGR-FG={operation.Foreground}]");
                    }
                    else if (operation.Background != null)
                    {
                        state.Append($"[SGR-BG={operation.Background}]");
                    }
                }
            }
        }
    }
}
