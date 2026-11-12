using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Tests;

public class BaseTest
{
    protected IWebDriver driver = null!;
    protected WebDriverWait wait = null!;

    [SetUp]
    public virtual void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public virtual void TearDown()
    {
        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                TakeScreenshotOnFailure();
            }
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Lỗi khi chụp ảnh màn hình: {ex.Message}");
        }
        finally
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }

    private void TakeScreenshotOnFailure()
    {
        if (driver is not ITakesScreenshot screenshotDriver)
        {
            return;
        }

        var screenshot = screenshotDriver.GetScreenshot();
        string repoRoot = FindRepoRoot(TestContext.CurrentContext.TestDirectory);
        string screenshotsDir = Path.Combine(repoRoot, "screenshots");

        if (!Directory.Exists(screenshotsDir))
        {
            Directory.CreateDirectory(screenshotsDir);
        }

        string rawTestName = TestContext.CurrentContext.Test.Name;
        string safeTestName = string.Join("_", rawTestName.Split(Path.GetInvalidFileNameChars()));
        safeTestName = Regex.Replace(safeTestName, @"\s+", "_");

        string filePath = Path.Combine(screenshotsDir, $"{safeTestName}.png");
        screenshot.SaveAsFile(filePath);

        TestContext.AddTestAttachment(filePath, $"Screenshot on failure: {safeTestName}");
        TestContext.Out.WriteLine($"Đã chụp ảnh lỗi và lưu tại: {filePath}");
    }

    private static string FindRepoRoot(string startDir)
    {
        var current = new DirectoryInfo(startDir);
        while (current != null)
        {
            if (Directory.Exists(Path.Combine(current.FullName, ".git")) ||
                current.GetFiles("*.sln*").Length > 0)
            {
                return current.FullName;
            }
            current = current.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}
