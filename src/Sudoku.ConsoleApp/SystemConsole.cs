namespace Sudoku.ConsoleApp;

/// <summary>Production <see cref="IGameConsole"/> backed by <see cref="Console"/>.</summary>
public sealed class SystemConsole : IGameConsole
{
    public string? ReadLine() => Console.ReadLine();

    public void WriteLine(string text = "") => Console.WriteLine(text);

    public ConsoleKey ReadKey() => Console.ReadKey(intercept: true).Key;
}
