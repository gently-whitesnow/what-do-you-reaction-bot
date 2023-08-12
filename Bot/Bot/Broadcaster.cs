using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bot.Inline;
using Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Bot;

public static class Broadcaster
{
    private static int SecondsRoundDuration = 60;

    private static readonly string SadnessStickerId =
        "CAACAgIAAxkBAAOhY5JNX9-5e46CM8_qfHgSO9xikOUAAkgAA61lvBSAc4rPFC0AAdwrBA";

    public static ITelegramBotClient IncludeBroadcaster(this ITelegramBotClient botClient)
    {
        Task.Factory.StartNew(async () =>
        {
            while (true)
            {
                Thread.Sleep(1000);
                foreach (var room in StaticStorage.Rooms.Values)
                {
                    if (string.IsNullOrEmpty(room.GuessSentence))
                        continue;
                    var seconds = DateTime.Now.Subtract(room.DateRoundStarted).Seconds;
                    var minutes = DateTime.Now.Subtract(room.DateRoundStarted).Minutes;
                    if (seconds + minutes * 60 >= SecondsRoundDuration || room.Medias.Count == room.Users.Count)
                    {
                        await RoundConclusionAsync(botClient, room);
                    }
                    else
                    {
                        // Допускаемая погрешность 1 секунда
                        if (seconds % 15 <= 1)
                            await SendAll(botClient, room, $"Осталось {SecondsRoundDuration - seconds} секунд!");
                    }
                }
            }
        }, TaskCreationOptions.LongRunning).ConfigureAwait(false);

        return botClient;
    }

    public static async Task SendAll(ITelegramBotClient botClient, Room.Room room, string message)
    {
        foreach (var user in room.Users)
        {
            if (!StaticStorage.Users.TryGetValue(user.ChatId, out var u))
                continue;
            await botClient.SendTextMessageAsync(u.ChatId, message, ParseMode.Html);
        }
    }

    public static async Task SendAllSticker(ITelegramBotClient botClient, Room.Room room, string userName,
        string sticker)
    {
        foreach (var user in room.Users)
        {
            if (!StaticStorage.Users.TryGetValue(user.ChatId, out var u))
                continue;
            await botClient.SendTextMessageAsync(u.ChatId, text: $"<b>{userName}</b>",
                ParseMode.Html);
            await botClient.SendStickerAsync(u.ChatId, sticker);
        }
    }

    public static async Task SendAllInlineBlocks(ITelegramBotClient botClient, Room.Room room)
    {
        foreach (var user in room.Users)
        {
            if (!StaticStorage.Users.TryGetValue(user.ChatId, out var u))
                continue;
            await InlineSender.SendRoomInlineKeyboard(botClient, user.ChatId);
        }
    }

    private static async Task RoundConclusionAsync(ITelegramBotClient botClient, Room.Room room)
    {
        await SendAll(botClient, room, $"Загаданная фраза:\n<b>{room.GuessSentence}</b>\n");
        room.GuessSentence = null;
        // Стикеров нет
        if (room.Medias.Count == 0)
        {
            await SendAllSticker(botClient, room, "Никто не прислал стикер", SadnessStickerId);
            await SendAllInlineBlocks(botClient, room);
            return;
        }

        //logging
        Interlocked.Increment(ref StaticStorage.RoundsCount);

        // Стикеры есть, рассылка всех стикеров
        foreach (var media in room.Medias)
        {
            await SendAllSticker(botClient, room, media.UserOwnerName, media.MediaId);
        }

        await SendAllInlineBlocks(botClient, room);
        room.Medias = new List<Media>();
    }
}