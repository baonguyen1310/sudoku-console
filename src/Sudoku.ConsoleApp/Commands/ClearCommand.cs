using Sudoku.Core;

namespace Sudoku.ConsoleApp.Commands;

/// <summary>Empties a cell, e.g. "C5 clear".</summary>
public sealed record ClearCommand(Position Position) : IGameCommand
{
    public CommandResult Execute(SudokuGame game)
    {
        try
        {
            game.Clear(Position);
            return new CommandResult($"Cell {Position.Label} cleared.", ShowGrid: true);
        }
        catch (InvalidMoveException exception)
        {
            return new CommandResult($"Invalid move. {exception.Message}", ShowGrid: true);
        }
    }
}
