using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bot.Room;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = Bot.Models.User;

namespace Bot.Inline;

public static class InlineReceiver
{
    public static async Task Listen(ITelegramBotClient botClient, CancellationToken cancellationToken,
        CallbackQuery callback, User user)
    {
        switch (Enum.Parse<Buttons>(callback.Data))
        {
            case Buttons.CreateRoom:
            {
                var room = RoomManager.GetRoom(user.RoomName);
                if (room != null)
                {
                    await botClient.SendTextMessageAsync(user.ChatId,
                        $"Вы уже в комнате");
                    return;
                }

                user.RoomName = RoomManager.CreateRoom(user);
                await botClient.SendTextMessageAsync(user.ChatId,
                    $"Комната - <b>{user.RoomName}</b> - создана", ParseMode.Html);
                await InlineSender.SendRoomInlineKeyboard(botClient, user.ChatId);
                break;
            }
            case Buttons.LeaveRoom:
            {
                var room = RoomManager.GetRoom(user.RoomName);
                if (room == null)
                {
                    await botClient.SendTextMessageAsync(user.ChatId,
                        $"Вы не в комнате");
                    return;
                }

                RoomManager.LeaveRoom(user);
                await botClient.SendTextMessageAsync(user.ChatId,
                    $"Вы покинули комнату");
                await botClient.SendTextMessageAsync(user.ChatId,
                    $"Правила просты: \n1. Создаешь/присоединяешься к комнате\n2. Обмениваешься реакциями с участниками");
                await InlineSender.SendHelloInlineKeyboard(botClient, user.ChatId);
                break;
            }
            case Buttons.EnterToGeneral:
            {
                var room = RoomManager.GetRoom(user.RoomName);
                if (room != null)
                {
                    await botClient.SendTextMessageAsync(user.ChatId,
                        $"Вы уже в комнате");
                    return;
                }

                await TextManager.EnterRoom(botClient, "99", user);
                break;
            }
            case Buttons.RandomPhrase:
            {
                var room = RoomManager.GetRoom(user.RoomName);
                if (room == null)
                {
                    await botClient.SendTextMessageAsync(user.ChatId,
                        $"Вы не в комнате");
                    return;
                }

                await TextManager.SetPhrase(botClient, room, user, Texts.GetRandom());
                break;
            }
            case Buttons.WhoInRoom:
            {
                var room = RoomManager.GetRoom(user.RoomName);
                if (room == null)
                {
                    await botClient.SendTextMessageAsync(user.ChatId,
                        $"Вы не в комнате");
                    return;
                }

                var message = new StringBuilder();
                var counter = 1;
                foreach (var userInRoom in room.Users)
                {
                    message.Append($"{counter++}) {userInRoom.UserName}\n");
                }

                await botClient.SendTextMessageAsync(user.ChatId, message.ToString());

                break;
            }
        }
    }
}