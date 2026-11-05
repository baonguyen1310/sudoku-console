namespace Sudoku.Core;

internal static class RandomExtensions
{
    /// <summary>Fisher-Yates shuffle.</summary>
    public static void ShuffleInPlace<T>(this Random random, IList<T> items)
    {
        for (var i = items.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (items[i], items[j]) = (items[j], items[i]);
        }
    }
}
