using Sudoku.ConsoleApp.Rendering;
using Sudoku.Core;

namespace Sudoku.ConsoleApp.Commands;

/// <summary>Reports rule violations in the current grid.</summary>
public sealed record CheckCommand : IGameCommand
{
    public const string NoViolationsMessage = "No rule violations detected.";

    public CommandResult Execute(SudokuGame game)
    {
        var violations = game.CheckRules();

        if (violations.Count == 0)
        {
            return new CommandResult(NoViolationsMessage);
        }

        var lines = violations.Select(ViolationDescriber.Describe);
        return new CommandResult(string.Join(Environment.NewLine, lines));
    }
}
