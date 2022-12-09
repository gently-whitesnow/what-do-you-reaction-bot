using System;
using System.Threading;
using System.Threading.Tasks;
using Bot.Inline;
using Bot.Room;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot;

public class TelegramBot
{
    private readonly ITelegramBotClient _bot;

    public TelegramBot(string botToken)
    {
        _bot = new TelegramBotClient(botToken);
    }

    public Task Execute()
    {
        Console.WriteLine("Запущен бот " + _bot.GetMeAsync().Result);
        
        return _bot.IncludeBroadcaster().ReceiveAsync(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery,
                }
            }
        );
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        // Get all messages
        if (update.Type == UpdateType.Message)
        {
            var message = update.Message;
            if (message == null)
                return;
            var user = UserManager.GetUser(message.Chat.Id, message.From);
            if (message.Text?.ToLower() == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat, "<b>Привет, я рад тебя видеть!</b>", ParseMode.Html);
                await botClient.SendStickerAsync(message.Chat,
                    "CAACAgIAAxkBAAMkY5DpJfIRFH2_D4NXX2xGrx4H1t0AAlEAA61lvBSGovabuPX5bisE");
                await botClient.SendTextMessageAsync(message.Chat,
                    $"Правила просты: \n1. Создаешь/присоединяешься к комнате\n2. Обмениваешься реакциями с участниками");
                await InlineSender.SendHelloInlineKeyboard(botClient, user.ChatId);

                return;
            }

            //Logging
            if (message.Text?.ToLower() == "/stat")
            {
                await botClient.SendTextMessageAsync(message.Chat,
                    $"Статистика:\nЗапущен - {StaticStorage.StartDate}\nСтикеров - <b>{StaticStorage.StickersSent}</b>\nИгроков - <b>{StaticStorage.UsersPlayed}</b>\nРаундов - <b>{StaticStorage.RoundsCount}</b>\n",
                    ParseMode.Html);

                return;
            }

            // Пытаемся достать комнату пользователя, в которой он уже есть
            var room = RoomManager.GetRoom(user.RoomName);

            // Если пользователь не в комнате и он ее ищет
            if (room == null)
            {
                await RoomManager.EnterRoom(botClient, message.Text, user);
                return;
            }

            // Пользователь прислал стикер
            if (message.Sticker != null)
            {
                await StickerManager.GetSticker(botClient, user, room, message.Sticker);
                return;
            }

            //  Пользователь уже в комнате, мы ожидаем от него фразы, либо ничего
            await PhraseManager.SetPhrase(botClient, room, user, message.Text);
            return;
        }

        // Get all callbacks
        if (update.Type == UpdateType.CallbackQuery)
        {
            var query = update.CallbackQuery;
            if (query == null)
                return;
            var user = UserManager.GetUser(query.From.Id, query.From);
            await InlineReceiver.Listen(botClient, cancellationToken, update.CallbackQuery, user);
        }
    }


    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }
}