using System;

namespace Bot;

public static class Texts
{
    private static readonly Random Rand = new();

    public static string GetRandom()
    {
        return Book[Rand.Next(0, Book.Length)];
    }

    private static readonly string[] Book =
    {
        "Когда тебе бабушка в детстве подарила тысячу рублей и ты решаешь, что с ними сделать",
        "Когда не можешь придумать фразу, чтобы начать следующий раунд"
    };
}