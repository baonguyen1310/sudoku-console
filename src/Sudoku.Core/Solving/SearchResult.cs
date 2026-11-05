namespace Sudoku.Core.Solving;

internal readonly record struct SearchResult(int SolutionCount, Grid? FirstSolution);
