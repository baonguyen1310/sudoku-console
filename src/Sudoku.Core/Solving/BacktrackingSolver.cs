namespace Sudoku.Core.Solving;

public sealed class BacktrackingSolver : ISolver
{
    private readonly Random? _random;

    public BacktrackingSolver(Random? random = null)
    {
        _random = random;
    }

    public Grid? Solve(Grid puzzle) =>
        new SolverSearch(puzzle, solutionLimit: 1, _random).Run().FirstSolution;

    public int CountSolutions(Grid puzzle, int limit)
    {
        if (limit < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "Limit must be at least 1.");
        }

        return new SolverSearch(puzzle, limit, _random).Run().SolutionCount;
    }
}
