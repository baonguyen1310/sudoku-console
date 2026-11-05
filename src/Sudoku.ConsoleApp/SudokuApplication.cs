using Sudoku.ConsoleApp.Commands;
using Sudoku.ConsoleApp.Rendering;
using Sudoku.Core;
using Sudoku.Core.Generation;

namespace Sudoku.ConsoleApp;

/// <summary>Top-level flow: welcome, play a game, and offer another round after a win.</summary>
public sealed class SudokuApplication
{
    public const string PlayAgainMessage = "Press any key to play again...";

    private readonly IGameConsole _console;
    private readonly IPuzzleGenerator _generator;
    private readonly Random _random;
    private readonly CommandParser _parser = new();
    private readonly GridRenderer _renderer = new();

    public SudokuApplication(IGameConsole console, IPuzzleGenerator generator, Random random)
    {
        _console = console;
        _generator = generator;
        _random = random;
    }

    public void Run()
    {
        _console.WriteLine("Welcome to Sudoku!");
        _console.WriteLine();

        var outcome = PlayRound();

        while (outcome == SessionOutcome.Solved && WantsToPlayAgain())
        {
            _console.WriteLine();
            outcome = PlayRound();
        }
    }

    private SessionOutcome PlayRound()
    {
        var game = new SudokuGame(_generator.Generate(), _random);
        return new GameSession(game, _console, _parser, _renderer).Run();
    }

    private bool WantsToPlayAgain()
    {
        _console.WriteLine(PlayAgainMessage);
        return _console.ReadKey() != ConsoleKey.Escape;
    }
}
