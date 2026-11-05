namespace Sudoku.Core;

public sealed class InvalidMoveException : Exception
{
    public InvalidMoveException(string message) : base(message)
    {
    }
}
