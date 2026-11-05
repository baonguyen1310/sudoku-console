using Sudoku.Core;

namespace Sudoku.ConsoleApp.Rendering;

/// <summary>Draws the grid as text, with column numbers across the top and row letters down the side.</summary>
public sealed class GridRenderer
{
    private const string EmptyCell = "_";

    public string Render(SudokuGame game)
    {
        var lines = new List<string> { RenderHeader() };
        lines.AddRange(Enumerable.Range(0, Grid.Size).Select(row => RenderRow(game, row)));

        return string.Join(Environment.NewLine, lines);
    }

    private static string RenderHeader() =>
        "    " + string.Join(' ', Enumerable.Range(1, Grid.Size));

    private static string RenderRow(SudokuGame game, int row)
    {
        var cells = Grid.RowPositions(row).Select(position => RenderCell(game.ValueAt(position)));
        return $"  {Position.RowLetter(row)} {string.Join(' ', cells)}";
    }

    private static string RenderCell(int value) =>
        value == Grid.Empty ? EmptyCell : value.ToString();
}
