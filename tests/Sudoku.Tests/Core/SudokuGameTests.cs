namespace Sudoku.Tests.Core;

public class SudokuGameTests
{
    [Fact]
    public void Place_EmptyCell_StoresNumber()
    {
        var game = TestPuzzles.ClassicGame();

        game.Place(TestPuzzles.At("A3"), 4);

        Assert.Equal(4, game.ValueAt(TestPuzzles.At("A3")));
    }

    [Fact]
    public void Place_OverwritesPlayerEntry()
    {
        var game = TestPuzzles.ClassicGame();
        game.Place(TestPuzzles.At("A3"), 4);

        game.Place(TestPuzzles.At("A3"), 9);

        Assert.Equal(9, game.ValueAt(TestPuzzles.At("A3")));
    }

    [Fact]
    public void Place_PrefilledCell_ThrowsAndKeepsValue()
    {
        var game = TestPuzzles.ClassicGame();

        var exception = Assert.Throws<InvalidMoveException>(() => game.Place(TestPuzzles.At("A1"), 6));

        Assert.Equal("A1 is pre-filled.", exception.Message);
        Assert.Equal(5, game.ValueAt(TestPuzzles.At("A1")));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(-3)]
    public void Place_NumberOutsideOneToNine_Throws(int number)
    {
        var game = TestPuzzles.ClassicGame();

        var exception = Assert.Throws<InvalidMoveException>(() => game.Place(TestPuzzles.At("A3"), number));

        Assert.Equal("Number must be between 1 and 9.", exception.Message);
    }

    [Fact]
    public void Place_ConflictingNumber_IsAcceptedAndReportedByCheck()
    {
        var game = TestPuzzles.ClassicGame();

        game.Place(TestPuzzles.At("A3"), 3);

        Assert.Contains(new RuleViolation(3, UnitKind.Row, 0), game.CheckRules());
    }

    [Fact]
    public void Clear_PlayerEntry_EmptiesCell()
    {
        var game = TestPuzzles.ClassicGame();
        game.Place(TestPuzzles.At("A3"), 4);

        game.Clear(TestPuzzles.At("A3"));

        Assert.Equal(Grid.Empty, game.ValueAt(TestPuzzles.At("A3")));
    }

    [Fact]
    public void Clear_PrefilledCell_Throws()
    {
        var game = TestPuzzles.ClassicGame();

        Assert.Throws<InvalidMoveException>(() => game.Clear(TestPuzzles.At("A1")));
    }

    [Fact]
    public void IsGiven_DistinguishesPrefilledFromEmptyCells()
    {
        var game = TestPuzzles.ClassicGame();

        Assert.True(game.IsGiven(TestPuzzles.At("A1")));
        Assert.False(game.IsGiven(TestPuzzles.At("A3")));
    }

    [Fact]
    public void RevealHint_FillsEmptyCellWithCorrectValue()
    {
        var game = TestPuzzles.ClassicGame();

        var hint = game.RevealHint();

        Assert.NotNull(hint);
        var expected = TestPuzzles.ClassicSolution[hint.Position.Row][hint.Position.Column] - '0';
        Assert.Equal(expected, hint.Value);
        Assert.Equal(expected, game.ValueAt(hint.Position));
        Assert.False(game.IsGiven(hint.Position));
    }

    [Fact]
    public void RevealHint_CellHoldingWrongValue_CorrectsIt()
    {
        var game = TestPuzzles.ClassicGame();
        FillWithSolution(game);
        game.Place(TestPuzzles.At("A3"), 9); // correct value is 4

        var hint = game.RevealHint();

        Assert.Equal(new Hint(TestPuzzles.At("A3"), 4), hint);
        Assert.Equal(4, game.ValueAt(TestPuzzles.At("A3")));
    }

    [Fact]
    public void RevealHint_EveryCellCorrect_ReturnsNull()
    {
        var game = TestPuzzles.ClassicGame();
        FillWithSolution(game);

        Assert.Null(game.RevealHint());
    }

    [Fact]
    public void RevealHint_RepeatedUntilExhausted_SolvesPuzzle()
    {
        var game = TestPuzzles.ClassicGame();

        while (game.RevealHint() is not null)
        {
        }

        Assert.True(game.IsSolved);
    }

    [Fact]
    public void IsSolved_NewGame_IsFalse()
    {
        Assert.False(TestPuzzles.ClassicGame().IsSolved);
    }

    [Fact]
    public void IsSolved_AllCellsCorrectlyFilled_IsTrue()
    {
        var game = TestPuzzles.ClassicGame();

        FillWithSolution(game);

        Assert.True(game.IsSolved);
    }

    [Fact]
    public void IsSolved_FullButWithViolation_IsFalse()
    {
        var game = TestPuzzles.ClassicGame();
        FillWithSolution(game);

        game.Place(TestPuzzles.At("A3"), 9); // row A already has a 9

        Assert.False(game.IsSolved);
    }

    private static void FillWithSolution(SudokuGame game)
    {
        foreach (var position in Grid.AllPositions().Where(position => !game.IsGiven(position)))
        {
            game.Place(position, TestPuzzles.ClassicSolution[position.Row][position.Column] - '0');
        }
    }
}
