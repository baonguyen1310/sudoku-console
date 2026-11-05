namespace Sudoku.Tests.Core;

public class GridTests
{
    [Fact]
    public void NewGrid_IsEmpty()
    {
        var grid = new Grid();

        Assert.Equal(0, grid.FilledCount);
        Assert.False(grid.IsFull);
    }

    [Fact]
    public void Indexer_SetsAndGetsValue()
    {
        var grid = new Grid();

        grid[TestPuzzles.At("C4")] = 7;

        Assert.Equal(7, grid[TestPuzzles.At("C4")]);
        Assert.Equal(1, grid.FilledCount);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void Indexer_ValueOutOfRange_Throws(int value)
    {
        var grid = new Grid();

        Assert.Throws<ArgumentOutOfRangeException>(() => grid[TestPuzzles.At("A1")] = value);
    }

    [Fact]
    public void Clone_ProducesIndependentCopy()
    {
        var original = TestPuzzles.ParseGrid(TestPuzzles.ClassicGivens);
        var copy = original.Clone();

        copy[TestPuzzles.At("A3")] = 4;

        Assert.Equal(Grid.Empty, original[TestPuzzles.At("A3")]);
    }

    [Fact]
    public void IsFull_AllCellsFilled_ReturnsTrue()
    {
        var grid = TestPuzzles.ParseGrid(TestPuzzles.ClassicSolution);

        Assert.True(grid.IsFull);
    }

    [Fact]
    public void AllPositions_ReturnsEightyOneDistinctCells()
    {
        Assert.Equal(81, Grid.AllPositions().Distinct().Count());
    }

    [Fact]
    public void SubgridPositions_ReturnsNineCellsInThatSubgrid()
    {
        var positions = Grid.SubgridPositions(4).ToList();

        Assert.Equal(9, positions.Count);
        Assert.All(positions, position => Assert.Equal(4, position.SubgridIndex));
    }
}
