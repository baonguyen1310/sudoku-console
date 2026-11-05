using Sudoku.ConsoleApp.Rendering;

namespace Sudoku.Tests.ConsoleApp;

public class ViolationDescriberTests
{
    [Fact]
    public void Describe_Row_UsesRowLetter()
    {
        var message = ViolationDescriber.Describe(new RuleViolation(3, UnitKind.Row, 0));

        Assert.Equal("Number 3 already exists in Row A.", message);
    }

    [Fact]
    public void Describe_Column_UsesOneBasedColumnNumber()
    {
        var message = ViolationDescriber.Describe(new RuleViolation(5, UnitKind.Column, 0));

        Assert.Equal("Number 5 already exists in Column 1.", message);
    }

    [Fact]
    public void Describe_Subgrid_ReportsTheSameSubgrid()
    {
        var message = ViolationDescriber.Describe(new RuleViolation(8, UnitKind.Subgrid, 0));

        Assert.Equal("Number 8 already exists in the same 3×3 subgrid.", message);
    }
}
