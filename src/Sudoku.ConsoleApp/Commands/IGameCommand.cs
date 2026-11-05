using Sudoku.Core;

namespace Sudoku.ConsoleApp.Commands;

public interface IGameCommand
{
    CommandResult Execute(SudokuGame game);
}
