using Sudoku.ConsoleApp.Commands;

namespace Sudoku.Tests.ConsoleApp;

public class CommandTests
{
    [Fact]
    public void PlaceCommand_ValidMove_ReportsAcceptedAndShowsGrid()
    {
        var game = TestPuzzles.ClassicGame();

        var result = new PlaceCommand(TestPuzzles.At("A3"), 4).Execute(game);

        Assert.Equal("Move accepted.", result.Message);
        Assert.True(result.ShowGrid);
        Assert.Equal(4, game.ValueAt(TestPuzzles.At("A3")));
    }

    [Fact]
    public void PlaceCommand_PrefilledCell_ReportsInvalidMove()
    {
        var game = TestPuzzles.ClassicGame();

        var result = new PlaceCommand(TestPuzzles.At("A1"), 6).Execute(game);

        Assert.Equal("Invalid move. A1 is pre-filled.", result.Message);
        Assert.True(result.ShowGrid);
    }

    [Fact]
    public void PlaceCommand_NumberOutOfRange_ReportsInvalidMove()
    {
        var game = TestPuzzles.ClassicGame();

        var result = new PlaceCommand(TestPuzzles.At("A3"), 10).Execute(game);

        Assert.Equal("Invalid move. Number must be between 1 and 9.", result.Message);
        Assert.Equal(Grid.Empty, game.ValueAt(TestPuzzles.At("A3")));
    }

    [Fact]
    public void ClearCommand_PlayerEntry_EmptiesCell()
    {
        var game = TestPuzzles.ClassicGame();
        game.Place(TestPuzzles.At("A3"), 4);

        var result = new ClearCommand(TestPuzzles.At("A3")).Execute(game);

        Assert.Equal("Cell A3 cleared.", result.Message);
        Assert.Equal(Grid.Empty, game.ValueAt(TestPuzzles.At("A3")));
    }

    [Fact]
    public void ClearCommand_PrefilledCell_ReportsInvalidMove()
    {
        var game = TestPuzzles.ClassicGame();

        var result = new ClearCommand(TestPuzzles.At("A1")).Execute(game);

        Assert.Equal("Invalid move. A1 is pre-filled.", result.Message);
        Assert.Equal(5, game.ValueAt(TestPuzzles.At("A1")));
    }

    [Fact]
    public void HintCommand_RevealsCorrectCell()
    {
        var game = TestPuzzles.ClassicGame();

        var result = new HintCommand().Execute(game);

        Assert.StartsWith("Hint: Cell ", result.Message);
        Assert.True(result.ShowGrid);
        Assert.Equal(50, CountEmptyCells(game)); // 51 empty cells at the start, one now filled
    }

    [Fact]
    public void CheckCommand_NoViolations_ReportsClean()
    {
        var result = new CheckCommand().Execute(TestPuzzles.ClassicGame());

        Assert.Equal("No rule violations detected.", result.Message);
    }

    [Fact]
    public void CheckCommand_WithViolation_DescribesIt()
    {
        var game = TestPuzzles.ClassicGame();
        game.Place(TestPuzzles.At("A3"), 3);

        var result = new CheckCommand().Execute(game);

        Assert.Contains("Number 3 already exists in Row A.", result.Message);
    }

    [Fact]
    public void QuitCommand_RequestsQuit()
    {
        var result = new QuitCommand().Execute(TestPuzzles.ClassicGame());

        Assert.True(result.QuitRequested);
    }

    private static int CountEmptyCells(SudokuGame game) =>
        Grid.AllPositions().Count(position => game.ValueAt(position) == Grid.Empty);
}
