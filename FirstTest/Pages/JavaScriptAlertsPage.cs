using OpenQA.Selenium;

namespace FirstTests.Pages;

public class JavaScriptAlertsPage : BasePage
{
    private const string DuongDanUrl = "https://the-internet.herokuapp.com/javascript_alerts";

    private readonly By nutJsAlert = By.CssSelector("button[onclick='jsAlert()']");
    private readonly By nutJsConfirm = By.CssSelector("button[onclick='jsConfirm()']");
    private readonly By nutJsPrompt = By.CssSelector("button[onclick='jsPrompt()']");
    private readonly By vungKetQua = By.Id("result");

    public JavaScriptAlertsPage(IWebDriver driver) : base(driver)
    {
    }

    /// <summary>
    /// Điều hướng tới trang Javascript Alerts.
    /// </summary>
    public JavaScriptAlertsPage MoTrang()
    {
        driver.Navigate().GoToUrl(DuongDanUrl);
        return this;
    }

    /// <summary>
    /// Bấm nút kích hoạt hộp thoại JS Alert.
    /// </summary>
    public JavaScriptAlertsPage BamNutAlert()
    {
        Tim(nutJsAlert).Click();
        return this;
    }

    /// <summary>
    /// Bấm nút kích hoạt hộp thoại JS Confirm.
    /// </summary>
    public JavaScriptAlertsPage BamNutConfirm()
    {
        Tim(nutJsConfirm).Click();
        return this;
    }

    /// <summary>
    /// Bấm nút kích hoạt hộp thoại JS Prompt.
    /// </summary>
    public JavaScriptAlertsPage BamNutPrompt()
    {
        Tim(nutJsPrompt).Click();
        return this;
    }

    /// <summary>
    /// Chờ alert hiển thị và đọc nội dung văn bản bên trong alert.
    /// </summary>
    public string LayNoiDungAlert()
    {
        var alert = wait.Until(d => d.SwitchTo().Alert())!;
        return alert.Text!;
    }

    /// <summary>
    /// Chấp nhận (Accept/OK) hộp thoại alert đang hiển thị.
    /// </summary>
    public JavaScriptAlertsPage ChapNhanAlert()
    {
        var alert = wait.Until(d => d.SwitchTo().Alert())!;
        alert.Accept();
        return this;
    }

    /// <summary>
    /// Hủy bỏ (Dismiss/Cancel) hộp thoại alert đang hiển thị.
    /// </summary>
    public JavaScriptAlertsPage HuyAlert()
    {
        var alert = wait.Until(d => d.SwitchTo().Alert())!;
        alert.Dismiss();
        return this;
    }

    /// <summary>
    /// Nhập nội dung văn bản vào hộp thoại prompt và nhấn OK để chấp nhận.
    /// </summary>
    public JavaScriptAlertsPage NhapVaChapNhanPrompt(string noiDung)
    {
        var alert = wait.Until(d => d.SwitchTo().Alert())!;
        alert.SendKeys(noiDung);
        alert.Accept();
        return this;
    }

    /// <summary>
    /// Lấy nội dung hiển thị trong khu vực kết quả của trang web.
    /// </summary>
    public string LayKetQua()
    {
        return Tim(vungKetQua).Text;
    }
}
