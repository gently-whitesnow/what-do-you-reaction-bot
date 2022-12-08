using System;
using Bot;

Console.WriteLine("Bot starting ...");

var bot = new TelegramBot(new Configuration().ReadSection<string>("botToken"));
await bot.Execute();
