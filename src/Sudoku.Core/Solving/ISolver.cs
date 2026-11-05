namespace Sudoku.Core.Solving;

public interface ISolver
{
    Grid? Solve(Grid puzzle);

    int CountSolutions(Grid puzzle, int limit);
}
