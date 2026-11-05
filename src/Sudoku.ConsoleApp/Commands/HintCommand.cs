using Sudoku.Core;

namespace Sudoku.ConsoleApp.Commands;

/// <summary>Reveals (and fills in) one correct cell.</summary>
public sealed record HintCommand : IGameCommand
{
    public CommandResult Execute(SudokuGame game)
    {
        var hint = game.RevealHint();

        return hint is null
            ? new CommandResult("No hint available: every cell is already correct.")
            : new CommandResult($"Hint: Cell {hint.Position.Label} = {hint.Value}", ShowGrid: true);
    }
}
