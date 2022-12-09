using Bot.Models;

namespace Bot;

public static class UserManager
{
    public static User GetUser(long userId, Telegram.Bot.Types.User telegramUser)
    {
        if (StaticStorage.Users.TryGetValue(userId, out var member))
            return member;
        
        //logging
        StaticStorage.UsersPlayed++;
        
        var user = new User(userId, telegramUser.Username ?? telegramUser.FirstName + " " + telegramUser.LastName);
        StaticStorage.Users.TryAdd(userId, user);
        return user;
    }
}