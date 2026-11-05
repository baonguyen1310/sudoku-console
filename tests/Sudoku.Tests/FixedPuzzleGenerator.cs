namespace Sudoku.Tests;

internal sealed class FixedPuzzleGenerator : IPuzzleGenerator
{
    private readonly Func<Puzzle> _factory;

    public FixedPuzzleGenerator(Func<Puzzle> factory)
    {
        _factory = factory;
    }

    public Puzzle Generate() => _factory();
}
