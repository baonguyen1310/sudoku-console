using Sudoku.Core;

namespace Sudoku.Tests;

internal static class TestPuzzles
{
    public static readonly string[] ClassicGivens =
    {
        "53__7____",
        "6__195___",
        "_98____6_",
        "8___6___3",
        "4__8_3__1",
        "7___2___6",
        "_6____28_",
        "___419__5",
        "____8__79"
    };

    public static readonly string[] ClassicSolution =
    {
        "534678912",
        "672195348",
        "198342567",
        "859761423",
        "426853791",
        "713924856",
        "961537284",
        "287419635",
        "345286179"
    };

    public static Puzzle Classic() => new(ParseGrid(ClassicGivens), ParseGrid(ClassicSolution));

    public static SudokuGame ClassicGame(int seed = 1) => new(Classic(), new Random(seed));

    public static Grid ParseGrid(params string[] rows)
    {
        var grid = new Grid();

        for (var row = 0; row < Grid.Size; row++)
        {
            for (var column = 0; column < Grid.Size; column++)
            {
                var character = rows[row][column];

                if (character is >= '1' and <= '9')
                {
                    grid[new Position(row, column)] = character - '0';
                }
            }
        }

        return grid;
    }

    public static Position At(string label) =>
        Position.TryParse(label, out var position)
            ? position
            : throw new ArgumentException($"Bad cell label '{label}'.", nameof(label));

    /// <summary>Commands that fill every empty cell of the classic puzzle correctly.</summary>
    public static IEnumerable<string> WinningMoves() =>
        Grid.AllPositions()
            .Where(position => ClassicGivens[position.Row][position.Column] == '_')
            .Select(position => $"{position.Label} {ClassicSolution[position.Row][position.Column]}");
}
