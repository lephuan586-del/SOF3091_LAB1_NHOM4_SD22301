using FirstTests.Config;
using FirstTests.Utilities;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace FirstTests.Core;

public abstract class BaseTest
{
    protected IWebDriver Driver = null!;
    protected TestConfig Config = null!;

    protected virtual string StartUrl => Config.BaseUrl;

    [SetUp]
    public virtual void Setup()
    {
        Config = ConfigReader.Current;
        Driver = DriverFactory.Create(Config);

        TestContext.Progress.WriteLine($"Browser: {Config.Browser}, Headless: {Config.Headless}");

        if (!string.IsNullOrWhiteSpace(StartUrl))
        {
            Driver.Navigate().GoToUrl(StartUrl);
        }
    }

    [TearDown]
    public virtual void TearDown()
    {
        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed && Driver != null)
            {
                ScreenshotHelper.Capture(Driver, TestContext.CurrentContext.Test.Name);
            }
        }
        catch (Exception ex)
        {
            TestContext.Progress.WriteLine($"Loi chup anh: {ex.Message}");
        }
        finally
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}
