using OpenQA.Selenium;

namespace FirstTests.Pages;

public class IframePage : BasePage
{
    private const string DuongDanUrl = "https://the-internet.herokuapp.com/iframe";

    private readonly By nutDongThongBao = By.CssSelector(".tox-notification__dismiss-button, button[aria-label='Close']");
    private readonly By khungIframe = By.Id("mce_0_ifr");
    private readonly By phanTuEditor = By.Id("tinymce");
    private readonly By tieuDeTrangChinh = By.TagName("h3");

    public IframePage(IWebDriver driver) : base(driver)
    {
    }

    /// <summary>
    /// Điều hướng tới trang iFrame và đóng thông báo nếu xuất hiện.
    /// </summary>
    public IframePage MoTrang()
    {
        driver.Navigate().GoToUrl(DuongDanUrl);
        DongThongBaoNeuCo();
        return this;
    }

    /// <summary>
    /// Chuyển ngữ cảnh vào bên trong iFrame để nhập nội dung văn bản mới vào trình soạn thảo.
    /// Tự động hoàn trả ngữ cảnh DefaultContent trong khối finally.
    /// </summary>
    public IframePage GoVaoEditor(string noiDung)
    {
        DongThongBaoNeuCo();
        Tim(khungIframe);

        try
        {
            driver.SwitchTo().Frame("mce_0_ifr");
            var editor = Tim(phanTuEditor);
            editor.Click();
            editor.Clear();
            editor.SendKeys(noiDung);
            wait.Until(d => editor.Text.Contains(noiDung));
        }
        finally
        {
            driver.SwitchTo().DefaultContent();
        }

        return this;
    }

    /// <summary>
    /// Lấy nội dung văn bản hiện tại trong trình soạn thảo TinyMCE bên trong iFrame.
    /// </summary>
    public string LayNoiDungEditor()
    {
        try
        {
            driver.SwitchTo().Frame("mce_0_ifr");
            return Tim(phanTuEditor).Text;
        }
        finally
        {
            driver.SwitchTo().DefaultContent();
        }
    }

    /// <summary>
    /// Lấy tiêu đề chính của trang web bên ngoài iFrame để xác nhận ngữ cảnh đã hoàn trả.
    /// </summary>
    public string LayTieuDeTrangChinh()
    {
        return Tim(tieuDeTrangChinh).Text;
    }

    private void DongThongBaoNeuCo()
    {
        try
        {
            var danhSachNut = driver.FindElements(nutDongThongBao);
            foreach (var nut in danhSachNut)
            {
                if (nut.Displayed)
                {
                    nut.Click();
                }
            }
        }
        catch (WebDriverException)
        {
            // Bỏ qua nếu banner không tồn tại hoặc đã đóng
        }
    }
}
