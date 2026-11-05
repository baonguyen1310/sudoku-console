using Sudoku.Core;

namespace Sudoku.ConsoleApp.Commands;

/// <summary>Enters a number into a cell, e.g. "B3 7".</summary>
public sealed record PlaceCommand(Position Position, int Number) : IGameCommand
{
    public CommandResult Execute(SudokuGame game)
    {
        try
        {
            game.Place(Position, Number);
            return new CommandResult("Move accepted.", ShowGrid: true);
        }
        catch (InvalidMoveException exception)
        {
            return new CommandResult($"Invalid move. {exception.Message}", ShowGrid: true);
        }
    }
}
