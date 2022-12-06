using Microsoft.Extensions.Configuration;

namespace Bot;

public class Configuration
{
    public T ReadSection<T>(string sectionName)
    {
        var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        var configurationRoot = builder.Build();
        
        return configurationRoot.GetSection(sectionName).Get<T>();
    }
}