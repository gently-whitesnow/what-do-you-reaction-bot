using System;
using System.Collections.Generic;
using Bot.Models;

namespace Bot.Room;

public class Room
{
    public Room(string roomName, User userCreator)
    {
        RoomName = roomName;
        if (userCreator != null)
            Users.Add(userCreator);
    }

    public string RoomName { get; set; }
    public List<User> Users { get; set; } = new List<User>();
    public List<Media> Medias { get; set; } = new List<Media>();
    public string GuessSentence { get; set; }
    public DateTime DateRoundStarted { get; set; }
}