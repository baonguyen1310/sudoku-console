namespace Sudoku.Tests.Core;

public class GridValidatorTests
{
    [Fact]
    public void FindViolations_CompleteValidGrid_ReturnsNone()
    {
        var grid = TestPuzzles.ParseGrid(TestPuzzles.ClassicSolution);

        Assert.Empty(GridValidator.FindViolations(grid));
    }

    [Fact]
    public void FindViolations_PartialValidGrid_IgnoresEmptyCells()
    {
        var grid = TestPuzzles.ParseGrid(TestPuzzles.ClassicGivens);

        Assert.Empty(GridValidator.FindViolations(grid));
    }

    [Fact]
    public void FindViolations_DuplicateInRow_ReportsRow()
    {
        var grid = TestPuzzles.ParseGrid(TestPuzzles.ClassicGivens);
        grid[TestPuzzles.At("A3")] = 3; // row A already has a 3 at A2

        var violations = GridValidator.FindViolations(grid);

        Assert.Contains(new RuleViolation(3, UnitKind.Row, 0), violations);
    }

    [Fact]
    public void FindViolations_DuplicateInColumn_ReportsColumn()
    {
        var grid = TestPuzzles.ParseGrid(TestPuzzles.ClassicGivens);
        grid[TestPuzzles.At("C1")] = 5; // column 1 already has a 5 at A1

        var violations = GridValidator.FindViolations(grid);

        Assert.Contains(new RuleViolation(5, UnitKind.Column, 0), violations);
    }

    [Fact]
    public void FindViolations_DuplicateInSubgrid_ReportsSubgrid()
    {
        var grid = TestPuzzles.ParseGrid(TestPuzzles.ClassicGivens);
        grid[TestPuzzles.At("B3")] = 8; // top-left subgrid already has an 8 at C3

        var violations = GridValidator.FindViolations(grid);

        Assert.Contains(new RuleViolation(8, UnitKind.Subgrid, 0), violations);
    }

    [Fact]
    public void FindViolations_DigitRepeatedThreeTimes_ReportsOnce()
    {
        var grid = new Grid();
        grid[TestPuzzles.At("A1")] = 4;
        grid[TestPuzzles.At("A5")] = 4;
        grid[TestPuzzles.At("A9")] = 4;

        var violations = GridValidator.FindViolations(grid);

        Assert.Single(violations, violation => violation.Unit == UnitKind.Row);
    }
}
