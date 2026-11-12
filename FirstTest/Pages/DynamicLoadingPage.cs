using OpenQA.Selenium;

namespace FirstTests.Pages;

public class DynamicLoadingPage : BasePage
{
    private const string DuongDanUrl = "https://the-internet.herokuapp.com/dynamic_loading/2";

    private readonly By nutBatDau = By.CssSelector("#start button");
    private readonly By phanTuKetQua = By.Id("finish");

    public DynamicLoadingPage(IWebDriver driver) : base(driver)
    {
    }

    /// <summary>
    /// Mở trang kiểm thử Dynamic Loading ví dụ 2.
    /// </summary>
    public DynamicLoadingPage MoTrang()
    {
        driver.Navigate().GoToUrl(DuongDanUrl);
        return this;
    }

    /// <summary>
    /// Bấm nút Start để kích hoạt quá trình tải nội dung bất đồng bộ.
    /// </summary>
    public DynamicLoadingPage BamStart()
    {
        Tim(nutBatDau).Click();
        return this;
    }

    /// <summary>
    /// Tìm phần tử kết quả không dùng cơ chế chờ đợi (dành cho ca test minh họa lỗi NoSuchElementException).
    /// </summary>
    public string LayKetQuaKhongCho()
    {
        return TimKhongCho(phanTuKetQua).Text;
    }

    /// <summary>
    /// Tìm phần tử kết quả có dùng Explicit Wait chờ phần tử xuất hiện trong DOM.
    /// </summary>
    public string LayKetQua()
    {
        return Tim(phanTuKetQua).Text;
    }
}
