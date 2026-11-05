using System.Text;
using Sudoku.ConsoleApp;
using Sudoku.Core.Generation;
using Sudoku.Core.Solving;

Console.OutputEncoding = Encoding.UTF8;

var random = new Random();
var generator = new PuzzleGenerator(new BacktrackingSolver(random), random);

new SudokuApplication(new SystemConsole(), generator, random).Run();
