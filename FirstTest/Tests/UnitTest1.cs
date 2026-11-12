using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests.Tests;

public class Tests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();          // mở Chrome
    }

    [Test]
    public void OpenExampleSite_TitleIsCorrect()
    {
        driver.Navigate().GoToUrl("https://example.com");   // điều hướng
        string title = driver.Title;                        // đọc tiêu đề
        Console.WriteLine("Tiêu đề trang: " + title);       // in tiêu đề

        Assert.That(title, Is.EqualTo("Example Domain"));   // kiểm tra kết quả
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();                        // đóng trình duyệt
        driver.Dispose();
    }
}
