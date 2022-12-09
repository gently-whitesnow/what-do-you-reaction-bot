using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Bot.Models;

namespace Bot;

public static class StaticStorage
{
    // userId-member
    public static ConcurrentDictionary<long, User> Users = new();

    // roomName - room
    // общая комната 99
    public static ConcurrentDictionary<string, Room.Room> Rooms = new(
        new Dictionary<string, Room.Room>
            {{"99", new Room.Room("99", null)}});

    public static int RoomsCount = 100;

    /// <summary>
    /// logging
    /// </summary>
    public static long RoundsCount = 0;

    public static long UsersPlayed = 0;
    public static long StickersSent = 0;
    public static DateTime StartDate = DateTime.Now;
}