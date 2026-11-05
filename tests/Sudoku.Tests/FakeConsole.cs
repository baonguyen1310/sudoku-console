using System.Text;
using Sudoku.ConsoleApp;

namespace Sudoku.Tests;

/// <summary>Scripted console: replays queued input lines and key presses and records all output.</summary>
internal sealed class FakeConsole : IGameConsole
{
    private readonly Queue<string> _lines;
    private readonly Queue<ConsoleKey> _keys;
    private readonly StringBuilder _output = new();

    public FakeConsole(IEnumerable<string> lines, params ConsoleKey[] keys)
    {
        _lines = new Queue<string>(lines);
        _keys = new Queue<ConsoleKey>(keys);
    }

    public string Output => _output.ToString();

    /// <summary>Returns null once the script runs out, like end-of-input on a real console.</summary>
    public string? ReadLine() => _lines.Count > 0 ? _lines.Dequeue() : null;

    public void WriteLine(string text = "") => _output.AppendLine(text);

    /// <summary>Presses Escape once the script runs out so a test can never loop forever.</summary>
    public ConsoleKey ReadKey() => _keys.Count > 0 ? _keys.Dequeue() : ConsoleKey.Escape;
}
