using System;

namespace Bot;

/// <summary>
/// Тут должны быть рандомные текста для игр
/// </summary>
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
        "Когда не можешь придумать фразу, чтобы начать следующий раунд",
        "Твоя реакция, когда в общественном месте, неожиданно слышишь свою любимую музыку",
        "Твоя реакция, когда пришло смс от сына с просьбой занять 1000 рублей, но у тебя нет детей",
        "Твоя реакция, когда не готовился к уроку, но сказал буквально пару слов, и тебе поставили 5",
        "Твоя реакция, когда нашёл нашел деньги в старой куртке",
        "Твоя реакция, когда ты нашёл 5000 тысяч на дороге, но они оказались фальшивыми",
        "Твоя реакция, когда учитель опаздывает на 15 минут, но при этом не отменяет контрольную, рассчитанную на 45 минут",
        "Твоя реакция, когда ты упрашиваешь маму взять домашнее животное домой",
        "Твоя реакция, когда тебе в сотый раз говорят очевидный факт",
        "Твоя реакция, когда ты продал Аляску, но потом узнают, что там находятся тонны нефти",
        "Когда учитель говорит: 'Ну раз ты такой умный, то выходи к доске и решай.' И ты выходишь к доске и решаешь пример",
        "Твоя реакция, когда ты искал медь, а нашёл золото",
        "Твоя реакция, когда тебе говорили, что ничего не сможешь, но ты всё смог",
    };
}