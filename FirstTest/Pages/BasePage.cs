using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver driver;
    protected readonly WebDriverWait wait;

    protected BasePage(IWebDriver driver)
    {
        this.driver = driver;
        this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    /// <summary>
    /// Tìm một phần tử có kèm Explicit Wait (chờ hiển thị và tương tác được).
    /// </summary>
    protected IWebElement Tim(By locator)
    {
        return wait.Until(d => d.FindElement(locator));
    }

    /// <summary>
    /// Tìm tất cả các phần tử khớp locator có kèm Explicit Wait (chờ ít nhất 1 phần tử xuất hiện).
    /// </summary>
    protected IReadOnlyCollection<IWebElement> TimTatCa(By locator)
    {
        return wait.Until(d =>
        {
            var elements = d.FindElements(locator);
            return elements.Count > 0 ? elements : null;
        })!;
    }

    /// <summary>
    /// Kiểm tra phần tử có hiển thị trên trang hay không.
    /// </summary>
    protected bool CoHienThi(By locator)
    {
        try
        {
            return Tim(locator).Displayed;
        }
        catch (WebDriverException)
        {
            return false;
        }
    }

    /// <summary>
    /// Tìm phần tử trực tiếp không qua cơ chế chờ (Explicit Wait), chỉ dùng cho bài test minh hoạ lỗi rớt.
    /// </summary>
    protected IWebElement TimKhongCho(By locator)
    {
        return driver.FindElement(locator);
    }
}
