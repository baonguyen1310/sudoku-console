using Sudoku.Core;
using Sudoku.Core.Rules;

namespace Sudoku.ConsoleApp.Rendering;

/// <summary>Turns a <see cref="RuleViolation"/> into a message for the player.</summary>
public static class ViolationDescriber
{
    public static string Describe(RuleViolation violation) => violation.Unit switch
    {
        UnitKind.Row => $"Number {violation.Number} already exists in Row {Position.RowLetter(violation.Index)}.",
        UnitKind.Column => $"Number {violation.Number} already exists in Column {violation.Index + 1}.",
        UnitKind.Subgrid => $"Number {violation.Number} already exists in the same 3×3 subgrid.",
        _ => throw new ArgumentOutOfRangeException(nameof(violation), violation.Unit, "Unknown unit kind.")
    };
}
