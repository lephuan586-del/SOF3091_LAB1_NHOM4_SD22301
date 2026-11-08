using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FirstTests;

public class BaseTest
{
    protected IWebDriver driver = null!;
    protected WebDriverWait wait = null!;

    [SetUp]
    public virtual void Setup()
    {
        // Khởi tạo ChromeDriver
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        // Tuân thủ quy tắc: Không dùng Implicit Wait (đặt về 0)
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

        // Khởi tạo WebDriverWait với timeout 10 giây
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public virtual void TearDown()
    {
        try
        {
            // 1. Chỉ chụp ảnh màn hình khi test thất bại
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
            // 3. Sau khi chụp (hoặc nếu pass) mới đóng trình duyệt
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

        // Tự dò đường lên thư mục gốc của repo (chứa .git hoặc .sln/.slnx)
        string repoRoot = FindRepoRoot(TestContext.CurrentContext.TestDirectory);
        string screenshotsDir = Path.Combine(repoRoot, "screenshots");

        if (!Directory.Exists(screenshotsDir))
        {
            Directory.CreateDirectory(screenshotsDir);
        }

        // Làm sạch tên ca test để tạo tên file hợp lệ
        string rawTestName = TestContext.CurrentContext.Test.Name;
        string safeTestName = string.Join("_", rawTestName.Split(Path.GetInvalidFileNameChars()));
        safeTestName = Regex.Replace(safeTestName, @"\s+", "_");

        string filePath = Path.Combine(screenshotsDir, $"{safeTestName}.png");

        // Lưu ảnh màn hình dưới dạng PNG
        File.WriteAllBytes(filePath, screenshot.AsByteArray);

        // Đính kèm ảnh vào báo cáo test của NUnit
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
