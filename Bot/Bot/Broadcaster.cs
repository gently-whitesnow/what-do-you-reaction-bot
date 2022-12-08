using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bot.Models;
using Bot.Room;
using Telegram.Bot;

namespace Bot;

public static class Broadcaster
{
    private static int SecondsRound = 60;

    public static async Task RunAsync(ITelegramBotClient botClient)
    {
        while (true)
        {
            Thread.Sleep(1000);
            foreach (var room in StaticStorage.Rooms.Values)
            {
                if (string.IsNullOrEmpty(room.GuessSentence))
                    continue;
                var seconds = DateTime.Now.Subtract(room.DateRoundStarted).Seconds;
                if (seconds >= SecondsRound || room.Medias.Count == room.Users.Count)
                {
                    await EndOfRound(botClient, room);
                }
                else
                {
                    // Допускаемая погрешность 1 секунда
                    if (seconds % 15 <= 1)
                        await RoomManager.SendAll(botClient, room, $"Осталось {SecondsRound - seconds} секунд!");
                }
            }
        }
    }

    private static async Task EndOfRound(ITelegramBotClient botClient, Room.Room room)
    {
        await RoomManager.SendAll(botClient, room,$"Загаданная фраза:\n<b>{room.GuessSentence}</b>\n" );
        room.GuessSentence = null;
        // Стикеров нет
        if (room.Medias.Count == 0)
        {
            await RoomManager.SendAllSticker(botClient, room, "Никто не прислал стикер",
                "CAACAgIAAxkBAAOhY5JNX9-5e46CM8_qfHgSO9xikOUAAkgAA61lvBSAc4rPFC0AAdwrBA");
            await RoomManager.SendAllInlineBlocks(botClient, room);
            return;
        }
        //logging
        StaticStorage.RoundsCount++;
        
        // Стикеры есть, всем рассылаю
        foreach (var media in room.Medias)
        {
            await RoomManager.SendAllSticker(botClient, room, media.UserOwnerName, media.MediaId);
        }

        await RoomManager.SendAllInlineBlocks(botClient, room);
        room.Medias = new List<Media>();
    }
}