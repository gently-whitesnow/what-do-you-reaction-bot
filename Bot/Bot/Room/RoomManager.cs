using System.Threading.Tasks;
using Bot.Inline;
using Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Bot.Room;

public static class RoomManager
{
    public static Room? GetRoom(string room)
    {
        if (string.IsNullOrEmpty(room))
            return null;
        StaticStorage.Rooms.TryGetValue(room, out var r);
        return r;
    }

    public static string CreateRoom(User user)
    {
        var roomName = (StaticStorage.RoomIds).ToString();
        StaticStorage.Rooms.TryAdd(roomName, new Room(roomName, user));
        StaticStorage.RoomIds += 13;
        return roomName;
    }

    public static void LeaveRoom(User user)
    {
        var room = GetRoom(user.RoomName);
        room?.Users.Remove(user);
        user.RoomName = null;
    }
    
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
}