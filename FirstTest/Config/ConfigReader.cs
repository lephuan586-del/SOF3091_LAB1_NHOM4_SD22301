using Microsoft.Extensions.Configuration;

namespace FirstTests.Config;

public static class ConfigReader
{
    private static readonly Lazy<TestConfig> _lazyConfig = new Lazy<TestConfig>(LoadConfig);

    public static TestConfig Current => _lazyConfig.Value;

    private static TestConfig LoadConfig()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "Config", "appsettings.json");

        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException(
                $"Khong tim thay tap tin cau hinh tai duong dan: {configPath}. Vui long kiem tra CopyToOutputDirectory trong file csproj.");
        }

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(configPath, optional: false, reloadOnChange: true)
            .Build();

        var config = configuration.Get<TestConfig>();

        if (config == null)
        {
            throw new InvalidOperationException(
                $"Khong the doc hoac bind du lieu cau hinh tu tap tin: {configPath}");
        }

        return config;
    }
}
