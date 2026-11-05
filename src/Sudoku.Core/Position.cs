namespace Sudoku.Core;

public readonly record struct Position
{
    public Position(int row, int column)
    {
        if (row is < 0 or >= Grid.Size)
        {
            throw new ArgumentOutOfRangeException(nameof(row), row, "Row must be between 0 and 8.");
        }

        if (column is < 0 or >= Grid.Size)
        {
            throw new ArgumentOutOfRangeException(nameof(column), column, "Column must be between 0 and 8.");
        }

        Row = row;
        Column = column;
    }

    public int Row { get; }

    public int Column { get; }

    public int SubgridIndex => Row / Grid.SubgridSize * Grid.SubgridSize + Column / Grid.SubgridSize;

    public string Label => $"{RowLetter(Row)}{Column + 1}";

    public static char RowLetter(int row) => (char)('A' + row);

    public static bool TryParse(string? text, out Position position)
    {
        position = default;

        if (text is not { Length: 2 })
        {
            return false;
        }

        var row = char.ToUpperInvariant(text[0]) - 'A';
        var column = text[1] - '1';
        if (row is < 0 or >= Grid.Size || column is < 0 or >= Grid.Size)
        {
            return false;
        }

        position = new Position(row, column);

        return true;
    }
}