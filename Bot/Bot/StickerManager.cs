using System.Linq;
using System.Threading.Tasks;
using Bot.Inline;
using Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = Bot.Models.User;

namespace Bot;

public static class StickerManager
{
    public static async Task GetSticker(ITelegramBotClient botClient, User user, Room.Room room, Sticker sticker)
    {
        // Раунд еще не начался
        if (string.IsNullOrEmpty(room.GuessSentence))
        {
            await botClient.SendTextMessageAsync(user.ChatId, $"Мы еще не начали игру, задай фразу");
            await InlineSender.SendRoomInlineKeyboard(botClient, user.ChatId);
            return;
        }
        // logging
        StaticStorage.StickersSent++;
        
        var savedSticker = room.Medias.FirstOrDefault(d => d.UserChatId == user.ChatId);
        if (savedSticker != null)
        {
            savedSticker.MediaId = sticker.FileId;
            await botClient.SendTextMessageAsync(user.ChatId, $"Перезаписал твой стикер, ждем остальных участников!");
            return;
        }
        
        room.Medias.Add(new Media(user.ChatId,user.UserName,sticker.FileId));
        await botClient.SendTextMessageAsync(user.ChatId, $"Я запомнил твой стикер, ждем остальных участников!");
        await Broadcaster.SendAll(botClient, room, $"<b>{user.UserName}</b> - стикер получен");
    }
}