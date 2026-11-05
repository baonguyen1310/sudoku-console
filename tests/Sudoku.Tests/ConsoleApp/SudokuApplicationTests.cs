using Sudoku.ConsoleApp;

namespace Sudoku.Tests.ConsoleApp;

/// <summary>End-to-end tests: real parser, renderer, game and session driven through a scripted console.</summary>
public class SudokuApplicationTests
{
    private static string Play(IEnumerable<string> inputLines, params ConsoleKey[] keys)
    {
        var console = new FakeConsole(inputLines, keys);
        var generator = new FixedPuzzleGenerator(TestPuzzles.Classic);

        new SudokuApplication(console, generator, new Random(1)).Run();

        return console.Output;
    }

    private static int CountOccurrences(string text, string value) =>
        text.Split(value).Length - 1;

    [Fact]
    public void Run_OnStart_ShowsWelcomeAndPuzzle()
    {
        var output = Play(new[] { "quit" });

        Assert.Contains("Welcome to Sudoku!", output);
        Assert.Contains("Here is your puzzle:", output);
        Assert.Contains("  A 5 3 _ _ 7 _ _ _ _", output);
        Assert.Contains(GameSession.Prompt, output);
    }

    [Fact]
    public void Run_Quit_EndsWithoutOfferingAnotherGame()
    {
        var output = Play(new[] { "quit" });

        Assert.Contains("Goodbye!", output);
        Assert.DoesNotContain(GameSession.SolvedMessage, output);
        Assert.DoesNotContain(SudokuApplication.PlayAgainMessage, output);
    }

    [Fact]
    public void Run_InputEnds_ExitsCleanly()
    {
        var output = Play(Array.Empty<string>());

        Assert.DoesNotContain(GameSession.SolvedMessage, output);
    }

    [Fact]
    public void Run_ValidMove_IsAcceptedAndShownInGrid()
    {
        var output = Play(new[] { "A3 4", "quit" });

        Assert.Contains("Move accepted.", output);
        Assert.Contains("Current grid:", output);
        Assert.Contains("  A 5 3 4 _ 7 _ _ _ _", output);
    }

    [Fact]
    public void Run_PrefilledCell_IsRejected()
    {
        var output = Play(new[] { "A1 6", "quit" });

        Assert.Contains("Invalid move. A1 is pre-filled.", output);
        Assert.Contains("  A 5 3 _ _ 7 _ _ _ _", output);
    }

    [Fact]
    public void Run_NumberOutOfRange_IsRejected()
    {
        var output = Play(new[] { "A3 10", "quit" });

        Assert.Contains("Invalid move. Number must be between 1 and 9.", output);
    }

    [Fact]
    public void Run_UnknownCommand_ShowsHelp()
    {
        var output = Play(new[] { "dance", "Z9 1", "quit" });

        Assert.Equal(2, CountOccurrences(output, GameSession.UnrecognizedCommandMessage));
    }

    [Fact]
    public void Run_CheckOnCleanGrid_ReportsNoViolations()
    {
        var output = Play(new[] { "A3 4", "check", "quit" });

        Assert.Contains("No rule violations detected.", output);
    }

    [Theory]
    [InlineData("A3 3", "Number 3 already exists in Row A.")]
    [InlineData("C1 5", "Number 5 already exists in Column 1.")]
    [InlineData("B3 8", "Number 8 already exists in the same 3×3 subgrid.")]
    public void Run_CheckAfterConflictingMove_ReportsViolation(string move, string expectedMessage)
    {
        var output = Play(new[] { move, "check", "quit" });

        Assert.Contains(expectedMessage, output);
    }

    [Fact]
    public void Run_ClearAfterMove_RemovesNumber()
    {
        var output = Play(new[] { "A3 4", "A3 clear", "quit" });

        var afterClear = output[output.IndexOf("Cell A3 cleared.", StringComparison.Ordinal)..];
        Assert.Contains("  A 5 3 _ _ 7 _ _ _ _", afterClear);
    }

    [Fact]
    public void Run_Hint_RevealsOneCorrectNumber()
    {
        var output = Play(new[] { "hint", "quit" });

        Assert.Matches(@"Hint: Cell [A-I][1-9] = [1-9]", output);
    }

    [Fact]
    public void Run_AllCellsFilledCorrectly_ShowsSuccessAndOffersAnotherGame()
    {
        var output = Play(TestPuzzles.WinningMoves(), ConsoleKey.Escape);

        Assert.Contains(GameSession.SolvedMessage, output);
        Assert.Contains(SudokuApplication.PlayAgainMessage, output);
        Assert.Equal(1, CountOccurrences(output, "Here is your puzzle:"));
    }

    [Fact]
    public void Run_SolvedThenKeyPressed_StartsNewGame()
    {
        var inputs = TestPuzzles.WinningMoves().Append("quit");

        var output = Play(inputs, ConsoleKey.Enter);

        Assert.Equal(2, CountOccurrences(output, "Here is your puzzle:"));
        Assert.Equal(1, CountOccurrences(output, GameSession.SolvedMessage));
    }

    [Fact]
    public void Run_HintsOnlyUntilNoneLeft_SolvesPuzzle()
    {
        var hints = Enumerable.Repeat("hint", 51);

        var output = Play(hints, ConsoleKey.Escape);

        Assert.Contains(GameSession.SolvedMessage, output);
    }

    [Fact]
    public void Run_FilledGridWithViolation_DoesNotCountAsSolved()
    {
        var moves = TestPuzzles.WinningMoves()
            .Where(move => !move.StartsWith("A3"))
            .Append("A3 9");

        var output = Play(moves);

        Assert.DoesNotContain(GameSession.SolvedMessage, output);
    }
}
