using Sudoku.Core.Rules;

namespace Sudoku.Core;

public sealed class SudokuGame
{
    private readonly Grid _givens;
    private readonly Grid _solution;
    private readonly Grid _current;
    private readonly Random _random;

    public SudokuGame(Puzzle puzzle, Random random)
    {
        ArgumentNullException.ThrowIfNull(puzzle);
        ArgumentNullException.ThrowIfNull(random);

        _givens = puzzle.Givens.Clone();
        _solution = puzzle.Solution.Clone();
        _current = puzzle.Givens.Clone();
        _random = random;
    }

    public bool IsSolved => _current.IsFull && CheckRules().Count == 0;

    public int ValueAt(Position position) => _current[position];

    public bool IsGiven(Position position) => _givens[position] != Grid.Empty;

    public void Place(Position position, int number)
    {
        if (number is < 1 or > Grid.Size)
        {
            throw new InvalidMoveException("Number must be between 1 and 9.");
        }

        EnsureEditable(position);
        _current[position] = number;
    }

    public void Clear(Position position)
    {
        EnsureEditable(position);
        _current[position] = Grid.Empty;
    }

    public Hint? RevealHint()
    {
        var candidates = Grid.AllPositions()
            .Where(position => _current[position] != _solution[position])
            .ToList();

        if (candidates.Count == 0)
        {
            return null;
        }

        var position = candidates[_random.Next(candidates.Count)];
        _current[position] = _solution[position];

        return new Hint(position, _solution[position]);
    }

    public IReadOnlyList<RuleViolation> CheckRules() => GridValidator.FindViolations(_current);

    private void EnsureEditable(Position position)
    {
        if (IsGiven(position))
        {
            throw new InvalidMoveException($"{position.Label} is pre-filled.");
        }
    }
}