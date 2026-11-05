using Sudoku.Core.Solving;

namespace Sudoku.Core.Generation;


public sealed class PuzzleGenerator : IPuzzleGenerator
{
    public const int DefaultClueCount = 30;
    public const int MinimumClueCount = 17; // no uniquely solvable puzzle with fewer clues is known

    private const int MaxAttempts = 100;

    private readonly ISolver _solver;
    private readonly Random _random;
    private readonly int _clueCount;


    public PuzzleGenerator(ISolver solver, Random random, int clueCount = DefaultClueCount)
    {
        ArgumentNullException.ThrowIfNull(solver);
        ArgumentNullException.ThrowIfNull(random);

        if (clueCount is < MinimumClueCount or > Grid.Size * Grid.Size)
        {
            throw new ArgumentOutOfRangeException(
                nameof(clueCount), clueCount, $"Clue count must be between {MinimumClueCount} and 81.");
        }

        _solver = solver;
        _random = random;
        _clueCount = clueCount;
    }

    public Puzzle Generate()
    {
        for (var attempt = 0; attempt < MaxAttempts; attempt++)
        {
            var solution = _solver.Solve(new Grid())
                           ?? throw new InvalidOperationException("The solver could not complete an empty grid.");
            var givens = solution.Clone();

            if (TryRemoveCells(givens))
            {
                return new Puzzle(givens, solution);
            }
        }

        throw new InvalidOperationException(
            $"Could not generate a unique puzzle with {_clueCount} clues after {MaxAttempts} attempts.");
    }

    private bool TryRemoveCells(Grid grid)
    {
        var positions = Grid.AllPositions().ToList();
        _random.ShuffleInPlace(positions);

        foreach (var position in positions)
        {
            if (grid.FilledCount <= _clueCount)
            {
                break;
            }

            RemoveIfStillUnique(grid, position);
        }

        return grid.FilledCount == _clueCount;
    }

    private void RemoveIfStillUnique(Grid grid, Position position)
    {
        var original = grid[position];
        grid[position] = Grid.Empty;

        if (_solver.CountSolutions(grid, limit: 2) != 1)
        {
            grid[position] = original;
        }
    }
}
