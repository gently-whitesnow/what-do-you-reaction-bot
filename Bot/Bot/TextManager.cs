using System;
using System.Threading;
using System.Threading.Tasks;
using Bot.Inline;
using Bot.Models;
using Bot.Room;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Bot;

public static class TextManager
{
    public static async Task EnterRoom(ITelegramBotClient botClient, string roomName,User user)
    {
        var roomFromMessage = RoomManager.GetRoom(roomName);
        // Не получилось найти комнату
        if (roomFromMessage == null)
        {
            await botClient.SendTextMessageAsync(user.ChatId, "Комната с таким названием не найдена");
            await InlineSender.SendHelloInlineKeyboard(botClient, user.ChatId);
            return;
        }

        if (roomFromMessage.Users.Contains(user) && user.RoomName == roomFromMessage.RoomName)
        {
            await botClient.SendTextMessageAsync(user.ChatId, $"Вы уже в комнате {roomFromMessage.RoomName}");
        }
        else
        {
            // Получилось
            user.RoomName = roomFromMessage.RoomName;
            roomFromMessage.Users.Add(user);
            await botClient.SendTextMessageAsync(user.ChatId, "Вы вошли в комнату");
        }
        if (string.IsNullOrEmpty(roomFromMessage.GuessSentence))
        {

            await InlineSender.SendRoomInlineKeyboard(botClient, user.ChatId);
        }
        else
        {
            await botClient.SendTextMessageAsync(user.ChatId,
                $"Игра идет, выбери стикер описывающий фразу: \n<b>{roomFromMessage.GuessSentence}</b>", ParseMode.Html);
        }
    }

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
            await RoomManager.SendAll(botClient, room, $"Игра началась, выбери стикер описывающий фразу: \n<b>{room.GuessSentence}</b>");
        }
    }
}