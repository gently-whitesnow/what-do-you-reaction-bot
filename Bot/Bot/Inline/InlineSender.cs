using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Inline;

public static class InlineSender
{
    public static async Task<Message> SendHelloInlineKeyboard(ITelegramBotClient botClient, long chatId)
    {
        InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
                // first row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Создать комнату", Buttons.CreateRoom.ToString()),
                    InlineKeyboardButton.WithCallbackData("Войти в общую комнату", Buttons.EnterToGeneral.ToString()),
                }
            });

        return await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "<b>Введи название комнаты в которую хочешь войти или:</b>",
            replyMarkup: inlineKeyboard,parseMode:ParseMode.Html);
    }

    public static async Task<Message> SendRoomInlineKeyboard(ITelegramBotClient botClient, long chatId)
    {
        InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
                // first row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Выйти", Buttons.LeaveRoom.ToString()),
                },
                // second row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Кто в комнате", Buttons.WhoInRoom.ToString()),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Задать случайную фразу", Buttons.RandomPhrase.ToString()),
                }
            });

        return await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "<b>Введите фразу, чтобы начать раунд:</b>",
            replyMarkup: inlineKeyboard,parseMode:ParseMode.Html);
    }
}