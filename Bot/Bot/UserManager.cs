using Bot.Models;

namespace Bot;

public static class UserManager
{
    public static User GetUser(long userId, string userName)
    {
        if (StaticStorage.Users.TryGetValue(userId, out var member))
            return member;
        //logging
        StaticStorage.UsersPlayed++;
        
        var user = new User(userId, userName);
        StaticStorage.Users.TryAdd(userId, user);
        return user;
    }
}