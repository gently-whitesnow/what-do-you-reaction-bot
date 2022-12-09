using System;
using System.Threading.Tasks;
using Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Bot;

public static class PhraseManager
{
    public static async Task SetPhrase(ITelegramBotClient botClient,Room.Room room, User user, string phrase)
    {
        if (!string.IsNullOrEmpty(room.GuessSentence))
        {
            await botClient.SendTextMessageAsync(user.ChatId,
                $"Игра идет, выбери стикер описывающий фразу: \n<b>{room.GuessSentence}</b>", ParseMode.Html);
            return;
        }

        if (string.IsNullOrEmpty(room.GuessSentence) && !string.IsNullOrEmpty(phrase))
        {
            room.GuessSentence = phrase;
            room.DateRoundStarted = DateTime.Now;
            await Broadcaster.SendAll(botClient, room, $"Игра началась, выбери стикер описывающий фразу: \n<b>{room.GuessSentence}</b>");
        }
    }
}