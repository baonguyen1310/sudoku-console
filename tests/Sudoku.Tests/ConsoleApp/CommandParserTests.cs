using Sudoku.ConsoleApp.Commands;

namespace Sudoku.Tests.ConsoleApp;

public class CommandParserTests
{
    private readonly CommandParser _parser = new();

    [Theory]
    [InlineData("A3 4", "A3", 4)]
    [InlineData("b3 7", "B3", 7)]
    [InlineData("  I9    1  ", "I9", 1)]
    [InlineData("A3 0", "A3", 0)]
    [InlineData("A3 10", "A3", 10)]
    public void Parse_CellAndNumber_ReturnsPlaceCommand(string input, string cell, int number)
    {
        var command = _parser.Parse(input);

        Assert.Equal<IGameCommand?>(new PlaceCommand(TestPuzzles.At(cell), number), command);
    }

    [Theory]
    [InlineData("C5 clear")]
    [InlineData("c5 CLEAR")]
    public void Parse_CellAndClear_ReturnsClearCommand(string input)
    {
        var command = _parser.Parse(input);

        Assert.Equal<IGameCommand?>(new ClearCommand(TestPuzzles.At("C5")), command);
    }

    [Theory]
    [InlineData("hint")]
    [InlineData("HINT")]
    public void Parse_Hint_ReturnsHintCommand(string input)
    {
        Assert.IsType<HintCommand>(_parser.Parse(input));
    }

    [Theory]
    [InlineData("check")]
    [InlineData("Check")]
    public void Parse_Check_ReturnsCheckCommand(string input)
    {
        Assert.IsType<CheckCommand>(_parser.Parse(input));
    }

    [Theory]
    [InlineData("quit")]
    [InlineData("QUIT")]
    public void Parse_Quit_ReturnsQuitCommand(string input)
    {
        Assert.IsType<QuitCommand>(_parser.Parse(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("hello")]
    [InlineData("A3")]
    [InlineData("A3 x")]
    [InlineData("A3 4 5")]
    [InlineData("J1 4")]
    [InlineData("A0 4")]
    [InlineData("hint now")]
    public void Parse_UnrecognizedInput_ReturnsNull(string? input)
    {
        Assert.Null(_parser.Parse(input));
    }
}
