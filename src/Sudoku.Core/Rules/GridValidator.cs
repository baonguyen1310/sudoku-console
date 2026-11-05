namespace Sudoku.Core.Rules;

public static class GridValidator
{
    public static IReadOnlyList<RuleViolation> FindViolations(Grid grid)
    {
        var violations = new List<RuleViolation>();

        for (var index = 0; index < Grid.Size; index++)
        {
            violations.AddRange(DuplicatesIn(grid, UnitKind.Row, index, Grid.RowPositions(index)));
        }

        for (var index = 0; index < Grid.Size; index++)
        {
            violations.AddRange(DuplicatesIn(grid, UnitKind.Column, index, Grid.ColumnPositions(index)));
        }

        for (var index = 0; index < Grid.Size; index++)
        {
            violations.AddRange(DuplicatesIn(grid, UnitKind.Subgrid, index, Grid.SubgridPositions(index)));
        }

        return violations;
    }

    private static IEnumerable<RuleViolation> DuplicatesIn(
        Grid grid, UnitKind kind, int index, IEnumerable<Position> unit) =>
        unit.Select(position => grid[position])
            .Where(value => value != Grid.Empty)
            .GroupBy(value => value)
            .Where(group => group.Count() > 1)
            .Select(group => new RuleViolation(group.Key, kind, index));
}
