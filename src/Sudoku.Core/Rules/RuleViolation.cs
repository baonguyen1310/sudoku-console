namespace Sudoku.Core.Rules;

public sealed record RuleViolation(int Number, UnitKind Unit, int Index);
