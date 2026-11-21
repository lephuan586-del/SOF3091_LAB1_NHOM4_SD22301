using System.Drawing;
using FirstTests.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace FirstTests.Core;

public static class DriverFactory
{
    public static IWebDriver Create(TestConfig cfg)
    {
        if (cfg == null)
        {
            throw new ArgumentNullException(nameof(cfg), "TestConfig khong duoc null.");
        }

        string browserName = (cfg.Browser ?? string.Empty).Trim().ToLowerInvariant();
        IWebDriver driver = browserName switch
        {
            "chrome" => new ChromeDriver(CreateChromeOptions(cfg)),
            "firefox" => new FirefoxDriver(CreateFirefoxOptions(cfg)),
            "edge" => new EdgeDriver(CreateEdgeOptions(cfg)),
            _ => throw new ArgumentException(
                $"Trinh duyet '{cfg.Browser}' khong duoc ho tro. Danh sach trinh duyet hop le bao gom: chrome, firefox, edge.")
        };

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

        if (!cfg.Headless)
        {
            driver.Manage().Window.Size = new Size(cfg.WindowWidth, cfg.WindowHeight);
        }

        return driver;
    }

    private static ChromeOptions CreateChromeOptions(TestConfig cfg)
    {
        var options = new ChromeOptions();

        // Tat popup luu / doi mat khau de tranh bi che giao dien
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);

        if (cfg.Headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument($"--window-size={cfg.WindowWidth},{cfg.WindowHeight}");
        }

        return options;
    }

    private static FirefoxOptions CreateFirefoxOptions(TestConfig cfg)
    {
        var options = new FirefoxOptions();

        if (cfg.Headless)
        {
            options.AddArgument("--headless");
            options.AddArgument($"--width={cfg.WindowWidth}");
            options.AddArgument($"--height={cfg.WindowHeight}");
        }

        return options;
    }

    private static EdgeOptions CreateEdgeOptions(TestConfig cfg)
    {
        var options = new EdgeOptions();

        // Tat popup luu / doi mat khau cho Edge
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);

        if (cfg.Headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument($"--window-size={cfg.WindowWidth},{cfg.WindowHeight}");
        }

        return options;
    }
}
