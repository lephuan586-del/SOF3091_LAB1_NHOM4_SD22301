using FirstTests.Config;
using OpenQA.Selenium;

namespace FirstTests.Pages;

public class ExamplePage : BasePage
{
    private static string DuongDanUrl => ConfigReader.Current.ExampleUrl;

    public ExamplePage(IWebDriver driver) : base(driver)
    {
    }

    /// <summary>
    /// Điều hướng người dùng đến trang example.com.
    /// </summary>
    public ExamplePage MoTrang()
    {
        driver.Navigate().GoToUrl(DuongDanUrl);
        return this;
    }

    /// <summary>
    /// Lấy tiêu đề thực tế của trang web.
    /// </summary>
    public string LayTieuDeTrang()
    {
        return driver.Title;
    }
}
