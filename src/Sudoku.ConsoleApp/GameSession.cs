using Sudoku.ConsoleApp.Commands;
using Sudoku.ConsoleApp.Rendering;
using Sudoku.Core;

namespace Sudoku.ConsoleApp;

public enum SessionOutcome
{
    Quit,
    Solved
}

/// <summary>Runs the read-command / execute / display loop for one game.</summary>
public sealed class GameSession
{
    public const string Prompt = "Enter command (e.g., A3 4, C5 clear, hint, check, quit):";
    public const string UnrecognizedCommandMessage =
        "Unrecognized command. Try e.g. A3 4, C5 clear, hint, check or quit.";
    public const string SolvedMessage = "You have successfully completed the Sudoku puzzle!";

    private readonly SudokuGame _game;
    private readonly IGameConsole _console;
    private readonly CommandParser _parser;
    private readonly GridRenderer _renderer;

    public GameSession(SudokuGame game, IGameConsole console, CommandParser parser, GridRenderer renderer)
    {
        _game = game;
        _console = console;
        _parser = parser;
        _renderer = renderer;
    }

    public SessionOutcome Run()
    {
        _console.WriteLine("Here is your puzzle:");
        _console.WriteLine(_renderer.Render(_game));

        while (true)
        {
            _console.WriteLine();
            _console.WriteLine(Prompt);

            var input = _console.ReadLine();

            if (input is null)
            {
                return SessionOutcome.Quit;
            }

            var result = Execute(input);
            Display(result);

            if (result.QuitRequested)
            {
                return SessionOutcome.Quit;
            }

            if (_game.IsSolved)
            {
                _console.WriteLine();
                _console.WriteLine(SolvedMessage);
                return SessionOutcome.Solved;
            }
        }
    }

    private CommandResult Execute(string input)
    {
        var command = _parser.Parse(input);

        return command is null
            ? new CommandResult(UnrecognizedCommandMessage)
            : command.Execute(_game);
    }

    private void Display(CommandResult result)
    {
        _console.WriteLine();
        _console.WriteLine(result.Message);

        if (!result.ShowGrid)
        {
            return;
        }

        _console.WriteLine();
        _console.WriteLine("Current grid:");
        _console.WriteLine(_renderer.Render(_game));
    }
}
