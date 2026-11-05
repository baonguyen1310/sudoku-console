namespace Sudoku.Tests.Core;

public class PuzzleGeneratorTests
{
    private static PuzzleGenerator CreateGenerator(int seed, int clueCount = PuzzleGenerator.DefaultClueCount)
    {
        var random = new Random(seed);
        return new PuzzleGenerator(new BacktrackingSolver(random), random, clueCount);
    }

    [Fact]
    public void Generate_ByDefault_ProducesThirtyClues()
    {
        var puzzle = CreateGenerator(seed: 1).Generate();

        Assert.Equal(30, puzzle.Givens.FilledCount);
    }

    [Fact]
    public void Generate_ProducesValidCompleteSolution()
    {
        var puzzle = CreateGenerator(seed: 2).Generate();

        Assert.True(puzzle.Solution.IsFull);
        Assert.Empty(GridValidator.FindViolations(puzzle.Solution));
    }

    [Fact]
    public void Generate_GivensAreSubsetOfSolution()
    {
        var puzzle = CreateGenerator(seed: 3).Generate();

        var givens = Grid.AllPositions().Where(position => puzzle.Givens[position] != Grid.Empty);

        Assert.All(givens, position => Assert.Equal(puzzle.Solution[position], puzzle.Givens[position]));
    }

    [Fact]
    public void Generate_PuzzleHasExactlyOneSolution()
    {
        var puzzle = CreateGenerator(seed: 4).Generate();

        var solutions = new BacktrackingSolver().CountSolutions(puzzle.Givens, limit: 2);

        Assert.Equal(1, solutions);
    }

    [Fact]
    public void Generate_CustomClueCount_IsRespected()
    {
        var puzzle = CreateGenerator(seed: 5, clueCount: 36).Generate();

        Assert.Equal(36, puzzle.Givens.FilledCount);
    }

    [Fact]
    public void Generate_DifferentSeeds_ProduceDifferentPuzzles()
    {
        var first = CreateGenerator(seed: 10).Generate();
        var second = CreateGenerator(seed: 11).Generate();

        var identical = Grid.AllPositions().All(position => first.Givens[position] == second.Givens[position]);

        Assert.False(identical);
    }

    [Theory]
    [InlineData(16)]
    [InlineData(82)]
    public void Constructor_ClueCountOutOfRange_Throws(int clueCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateGenerator(seed: 1, clueCount));
    }
}
