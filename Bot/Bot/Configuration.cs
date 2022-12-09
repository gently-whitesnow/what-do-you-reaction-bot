using Microsoft.Extensions.Configuration;

namespace Bot;

public static class Configuration
{
    public static T ReadSection<T>(string sectionName)
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build()
            .GetSection(sectionName).Get<T>();
    }
}