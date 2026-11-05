using Sudoku.Core;

namespace Sudoku.ConsoleApp.Commands;

public sealed record QuitCommand : IGameCommand
{
    public CommandResult Execute(SudokuGame game) => new("Goodbye!", QuitRequested: true);
}
