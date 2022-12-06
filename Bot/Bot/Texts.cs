namespace Bot;

public static class Texts
{
    private static readonly Random Rand = new();

    public static string GetRandom()
    {
        return Book[Rand.Next(0, Book.Length)];
    }

    private static readonly string[] Book = new[]
    {
        "a", "b", "c", "b", "c", "b", "c", "b", "c", "b", "c", "b", "c"
    };
}