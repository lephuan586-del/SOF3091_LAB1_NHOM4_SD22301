using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace FirstTests.Utilities;

public static class ScreenshotHelper
{
    public static string? Capture(IWebDriver driver, string testName)
    {
        if (driver is not ITakesScreenshot screenshotDriver)
        {
            return null;
        }

        try
        {
            var screenshot = screenshotDriver.GetScreenshot();

            string repoRoot = FindRepoRoot(TestContext.CurrentContext.TestDirectory);
            string screenshotsDir = Path.Combine(repoRoot, "TestResults", "Screenshots");

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            // Lam sach cac ky tu khong hop le trong ten tap tin
            string safeTestName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
            safeTestName = Regex.Replace(safeTestName, @"[<>:'""/\\|?*]+", "_");
            safeTestName = Regex.Replace(safeTestName, @"\s+", "_").Trim('_');

            // Cat ngan neu ten qua dai
            if (safeTestName.Length > 60)
            {
                safeTestName = safeTestName.Substring(0, 60);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"{safeTestName}_{timestamp}.png";
            string filePath = Path.Combine(screenshotsDir, fileName);

            screenshot.SaveAsFile(filePath);

            TestContext.AddTestAttachment(filePath, $"Screenshot on failure: {safeTestName}");
            TestContext.Progress.WriteLine($"Da chup anh loi va luu tai: {filePath}");

            return filePath;
        }
        catch (Exception ex)
        {
            TestContext.Progress.WriteLine($"Loi trong qua trinh chup anh man hinh: {ex.Message}");
            return null;
        }
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

        return AppContext.BaseDirectory;
    }
}
