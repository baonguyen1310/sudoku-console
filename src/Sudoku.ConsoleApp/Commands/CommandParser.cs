using System.Globalization;
using Sudoku.Core;

namespace Sudoku.ConsoleApp.Commands;

/// <summary>
/// Turns a line of user input into a command. Accepted forms (case-insensitive):
/// "B3 7", "C5 clear", "hint", "check", "quit".
/// </summary>
public sealed class CommandParser
{
    private const string ClearKeyword = "clear";

    /// <summary>Returns null when the input is not a recognised command.</summary>
    public IGameCommand? Parse(string? input)
    {
        var tokens = (input ?? string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return tokens switch
        {
            [var keyword] => ParseKeyword(keyword),
            [var cell, var argument] => ParseCellCommand(cell, argument),
            _ => null
        };
    }

    private static IGameCommand? ParseKeyword(string keyword) => keyword.ToLowerInvariant() switch
    {
        "hint" => new HintCommand(),
        "check" => new CheckCommand(),
        "quit" => new QuitCommand(),
        _ => null
    };

    private static IGameCommand? ParseCellCommand(string cell, string argument)
    {
        if (!Position.TryParse(cell, out var position))
        {
            return null;
        }

        if (argument.Equals(ClearKeyword, StringComparison.OrdinalIgnoreCase))
        {
            return new ClearCommand(position);
        }

        return int.TryParse(argument, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number)
            ? new PlaceCommand(position, number)
            : null;
    }
}
