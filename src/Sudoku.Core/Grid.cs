namespace Sudoku.Core;

public sealed class Grid
{
    public const int Size = 9;
    public const int SubgridSize = 3;
    public const int Empty = 0;

    private readonly int[,] _cells;

    public Grid() : this(new int[Size, Size])
    {
    }

    private Grid(int[,] cells)
    {
        _cells = cells;
    }

    public int this[Position position]
    {
        get => _cells[position.Row, position.Column];
        set
        {
            if (value is < Empty or > Size)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Cell value must be between 0 and 9.");
            }

            _cells[position.Row, position.Column] = value;
        }
    }

    public int FilledCount => AllPositions().Count(position => this[position] != Empty);

    public bool IsFull => FilledCount == Size * Size;

    public Grid Clone() => new((int[,])_cells.Clone());

    public static IEnumerable<Position> AllPositions() =>
        Enumerable.Range(0, Size * Size).Select(index => new Position(index / Size, index % Size));

    public static IEnumerable<Position> RowPositions(int row) =>
        Enumerable.Range(0, Size).Select(column => new Position(row, column));

    public static IEnumerable<Position> ColumnPositions(int column) =>
        Enumerable.Range(0, Size).Select(row => new Position(row, column));

    public static IEnumerable<Position> SubgridPositions(int subgridIndex)
    {
        var top = subgridIndex / SubgridSize * SubgridSize;
        var left = subgridIndex % SubgridSize * SubgridSize;

        return Enumerable.Range(0, Size)
            .Select(offset => new Position(top + offset / SubgridSize, left + offset % SubgridSize));
    }
}
