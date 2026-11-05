namespace Sudoku.ConsoleApp;

/// <summary>The few console operations the game needs, so the game loop can be tested without a terminal.</summary>
public interface IGameConsole
{
    /// <summary>Returns the next line of input, or null when input has ended.</summary>
    string? ReadLine();

    void WriteLine(string text = "");

    ConsoleKey ReadKey();
}
