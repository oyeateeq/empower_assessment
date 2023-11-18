using Microsoft.Extensions.Configuration;

public class ConfigurationLoader
{
    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
    public IConfigurationRoot LoadConfiguration() => new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile(fullPath)
            .Build();
}
