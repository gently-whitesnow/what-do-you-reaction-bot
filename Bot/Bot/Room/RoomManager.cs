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

    public static async Task SendAll(ITelegramBotClient botClient, Room room, string message)
    {
        foreach (var user in room.Users)
        {
            if (!StaticStorage.Users.TryGetValue(user.ChatId, out var u))
                continue;
            await botClient.SendTextMessageAsync(u.ChatId, message, ParseMode.Html);
        }
    }

    public static async Task SendAllSticker(ITelegramBotClient botClient, Room room, string userName, string sticker)
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

    public static async Task SendAllInlineBlocks(ITelegramBotClient botClient, Room room)
    {
        foreach (var user in room.Users)
        {
            if (!StaticStorage.Users.TryGetValue(user.ChatId, out var u))
                continue;
            await InlineSender.SendRoomInlineKeyboard(botClient, user.ChatId);
        }
    }

    public static string CreateRoom(User user)
    {
        var roomName = (StaticStorage.RoomsCount).ToString();
        StaticStorage.Rooms.Add(roomName, new Room(roomName, user));
        StaticStorage.RoomsCount += 13;
        return roomName;
    }

    public static void LeaveRoom(User user)
    {
        var room = GetRoom(user.RoomName);
        room.Users.Remove(user);
        user.RoomName = null;
    }
}