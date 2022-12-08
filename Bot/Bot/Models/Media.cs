namespace Bot.Models;

public class Media
{
    public Media(long userChatId, string userOwnerName, string mediaId)
    {
        UserChatId = userChatId;
        UserOwnerName = userOwnerName;
        MediaId = mediaId;
    }
    public long UserChatId { get; set; }
    public string UserOwnerName { get; set; }
    public string MediaId { get; set; }
}