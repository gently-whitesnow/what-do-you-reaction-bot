// See https://aka.ms/new-console-template for more information

using Bot;


var conf = new Configuration();

Console.WriteLine(conf.ReadSection<string>("botToken"));