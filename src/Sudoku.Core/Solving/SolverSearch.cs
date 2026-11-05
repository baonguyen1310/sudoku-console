using System.Numerics;

namespace Sudoku.Core.Solving;

internal sealed class SolverSearch
{
    private const int CellCount = Grid.Size * Grid.Size;
    private const int NoEmptyCell = -1;
    private const int AllDigitsMask = 0b11_1111_1110; // bits 1..9

    private readonly int[] _cells = new int[CellCount];
    private readonly int[] _rowMasks = new int[Grid.Size];
    private readonly int[] _columnMasks = new int[Grid.Size];
    private readonly int[] _subgridMasks = new int[Grid.Size];
    private readonly Random? _random;
    private readonly int _solutionLimit;
    private readonly bool _givensAreConsistent;

    private int _solutionCount;
    private Grid? _firstSolution;

    public SolverSearch(Grid puzzle, int solutionLimit, Random? random)
    {
        _solutionLimit = solutionLimit;
        _random = random;
        _givensAreConsistent = TryLoadGivens(puzzle);
    }

    public SearchResult Run()
    {
        if (_givensAreConsistent)
        {
            Search();
        }

        return new SearchResult(_solutionCount, _firstSolution);
    }

    private void Search()
    {
        var cell = FindMostConstrainedEmptyCell();

        if (cell == NoEmptyCell)
        {
            RecordSolution();
            return;
        }

        foreach (var digit in CandidateDigits(cell))
        {
            Place(cell, digit);
            Search();
            Remove(cell, digit);

            if (_solutionCount >= _solutionLimit)
            {
                return;
            }
        }
    }

    private bool TryLoadGivens(Grid puzzle)
    {
        foreach (var position in Grid.AllPositions())
        {
            var digit = puzzle[position];

            if (digit == Grid.Empty)
            {
                continue;
            }

            var cell = position.Row * Grid.Size + position.Column;

            if ((CandidateMask(cell) & (1 << digit)) == 0)
            {
                return false;
            }

            Place(cell, digit);
        }

        return true;
    }

    private int FindMostConstrainedEmptyCell()
    {
        var bestCell = NoEmptyCell;
        var bestCount = int.MaxValue;

        for (var cell = 0; cell < CellCount; cell++)
        {
            if (_cells[cell] != Grid.Empty)
            {
                continue;
            }

            var count = BitOperations.PopCount((uint)CandidateMask(cell));

            if (count < bestCount)
            {
                bestCell = cell;
                bestCount = count;

                if (count <= 1)
                {
                    break;
                }
            }
        }

        return bestCell;
    }

    private List<int> CandidateDigits(int cell)
    {
        var mask = CandidateMask(cell);
        var digits = new List<int>();

        for (var digit = 1; digit <= Grid.Size; digit++)
        {
            if ((mask & (1 << digit)) != 0)
            {
                digits.Add(digit);
            }
        }

        _random?.ShuffleInPlace(digits);
        return digits;
    }

    private int CandidateMask(int cell) =>
        AllDigitsMask & ~(_rowMasks[cell / Grid.Size]
                          | _columnMasks[cell % Grid.Size]
                          | _subgridMasks[SubgridOf(cell)]);

    private void Place(int cell, int digit)
    {
        var bit = 1 << digit;
        _cells[cell] = digit;
        _rowMasks[cell / Grid.Size] |= bit;
        _columnMasks[cell % Grid.Size] |= bit;
        _subgridMasks[SubgridOf(cell)] |= bit;
    }

    private void Remove(int cell, int digit)
    {
        var bit = ~(1 << digit);
        _cells[cell] = Grid.Empty;
        _rowMasks[cell / Grid.Size] &= bit;
        _columnMasks[cell % Grid.Size] &= bit;
        _subgridMasks[SubgridOf(cell)] &= bit;
    }

    private void RecordSolution()
    {
        _solutionCount++;
        _firstSolution ??= ToGrid();
    }

    private Grid ToGrid()
    {
        var grid = new Grid();

        for (var cell = 0; cell < CellCount; cell++)
        {
            grid[new Position(cell / Grid.Size, cell % Grid.Size)] = _cells[cell];
        }

        return grid;
    }

    private static int SubgridOf(int cell)
    {
        var row = cell / Grid.Size;
        var column = cell % Grid.Size;
        return row / Grid.SubgridSize * Grid.SubgridSize + column / Grid.SubgridSize;
    }
}
