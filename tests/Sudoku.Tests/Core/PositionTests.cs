namespace Sudoku.Tests.Core;

public class PositionTests
{
    [Theory]
    [InlineData(0, 0, "A1")]
    [InlineData(1, 2, "B3")]
    [InlineData(8, 8, "I9")]
    public void Label_ValidCell_ReturnsRowLetterAndColumnNumber(int row, int column, string expected)
    {
        var position = new Position(row, column);

        Assert.Equal(expected, position.Label);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(9, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 9)]
    public void Constructor_OutOfRange_Throws(int row, int column)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Position(row, column));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(2, 2, 0)]
    [InlineData(0, 3, 1)]
    [InlineData(4, 4, 4)]
    [InlineData(8, 8, 8)]
    public void SubgridIndex_ReturnsContainingSubgrid(int row, int column, int expected)
    {
        Assert.Equal(expected, new Position(row, column).SubgridIndex);
    }

    [Theory]
    [InlineData("A1", 0, 0)]
    [InlineData("b3", 1, 2)]
    [InlineData("I9", 8, 8)]
    public void TryParse_ValidLabel_ReturnsPosition(string text, int row, int column)
    {
        var parsed = Position.TryParse(text, out var position);

        Assert.True(parsed);
        Assert.Equal(new Position(row, column), position);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("A0")]
    [InlineData("A10")]
    [InlineData("J1")]
    [InlineData("11")]
    [InlineData("AA")]
    public void TryParse_InvalidLabel_ReturnsFalse(string? text)
    {
        Assert.False(Position.TryParse(text, out _));
    }
}
