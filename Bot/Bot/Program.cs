using System;
using Bot;

Console.WriteLine("Bot starting ...");

var bot = new TelegramBot(Configuration.ReadSection<string>("botToken"));
await bot.Execute();
