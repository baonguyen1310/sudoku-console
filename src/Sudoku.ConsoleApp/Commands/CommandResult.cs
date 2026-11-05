namespace Sudoku.ConsoleApp.Commands;

/// <summary>What to tell the player after a command ran.</summary>
public sealed record CommandResult(string Message, bool ShowGrid = false, bool QuitRequested = false);
