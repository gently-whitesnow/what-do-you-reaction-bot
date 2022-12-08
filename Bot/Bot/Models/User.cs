namespace Bot.Models;

public class User
{
    public User(long chatId, string userName)
    {
        ChatId = chatId;
        UserName = userName;
    }

    public string RoomName { get; set; }
    public long ChatId { get; set; }
    public string UserName { get; set; }
}