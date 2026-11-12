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
        // Khá»Ÿi táº¡o ChromeDriver
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        // TuÃ¢n thá»§ quy táº¯c: KhÃ´ng dÃ¹ng Implicit Wait (Ä‘áº·t vá» 0)
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

        // Khá»Ÿi táº¡o WebDriverWait vá»›i timeout 10 giÃ¢y
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public virtual void TearDown()
    {
        try
        {
            // 1. Chá»‰ chá»¥p áº£nh mÃ n hÃ¬nh khi test tháº¥t báº¡i
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                TakeScreenshotOnFailure();
            }
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Lá»—i khi chá»¥p áº£nh mÃ n hÃ¬nh: {ex.Message}");
        }
        finally
        {
            // 3. Sau khi chá»¥p (hoáº·c náº¿u pass) má»›i Ä‘Ã³ng trÃ¬nh duyá»‡t
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

        // Tá»± dÃ² Ä‘Æ°á»ng lÃªn thÆ° má»¥c gá»‘c cá»§a repo (chá»©a .git hoáº·c .sln/.slnx)
        string repoRoot = FindRepoRoot(TestContext.CurrentContext.TestDirectory);
        string screenshotsDir = Path.Combine(repoRoot, "screenshots");

        if (!Directory.Exists(screenshotsDir))
        {
            Directory.CreateDirectory(screenshotsDir);
        }

        // LÃ m sáº¡ch tÃªn ca test Ä‘á»ƒ táº¡o tÃªn file há»£p lá»‡
        string rawTestName = TestContext.CurrentContext.Test.Name;
        string safeTestName = string.Join("_", rawTestName.Split(Path.GetInvalidFileNameChars()));
        safeTestName = Regex.Replace(safeTestName, @"\s+", "_");

        string filePath = Path.Combine(screenshotsDir, $"{safeTestName}.png");

        // LÆ°u áº£nh mÃ n hÃ¬nh dÆ°á»›i dáº¡ng PNG
        File.WriteAllBytes(filePath, screenshot.AsByteArray);

        // ÄÃ­nh kÃ¨m áº£nh vÃ o bÃ¡o cÃ¡o test cá»§a NUnit
        TestContext.AddTestAttachment(filePath, $"Screenshot on failure: {safeTestName}");
        TestContext.Out.WriteLine($"ÄÃ£ chá»¥p áº£nh lá»—i vÃ  lÆ°u táº¡i: {filePath}");
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
