using OpenQA.Selenium;

namespace FirstTests.Pages;

public class LoginPage : BasePage
{
    private const string DuongDanUrl = "https://www.saucedemo.com";

    private readonly By truongTaiKhoan = By.Id("user-name");
    private readonly By truongMatKhau = By.Id("password");
    private readonly By nutDangNhap = By.Id("login-button");
    private readonly By hopThongBaoLoi = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver)
    {
    }

    /// <summary>
    /// Điều hướng người dùng đến trang đăng nhập saucedemo.
    /// </summary>
    public LoginPage MoTrang()
    {
        driver.Navigate().GoToUrl(DuongDanUrl);
        return this;
    }

    /// <summary>
    /// Thực hiện đăng nhập với thông tin tài khoản hợp lệ, chuyển tiếp sang trang Inventory.
    /// </summary>
    public InventoryPage DangNhap(string tk, string mk)
    {
        Tim(truongTaiKhoan).SendKeys(tk);
        Tim(truongMatKhau).SendKeys(mk);
        Tim(nutDangNhap).Click();
        return new InventoryPage(driver);
    }

    /// <summary>
    /// Thiết kế đăng nhập không thành công trả về chính LoginPage (this) vì hệ thống
    /// giữ nguyên người dùng ở trang đăng nhập và hiển thị thông báo lỗi, cho phép gọi tiếp LayThongBaoLoi().
    /// </summary>
    public LoginPage DangNhapKhongThanhCong(string tk, string mk)
    {
        Tim(truongTaiKhoan).SendKeys(tk);
        Tim(truongMatKhau).SendKeys(mk);
        Tim(nutDangNhap).Click();
        return this;
    }

    /// <summary>
    /// Kiểm tra hộp thoại thông báo lỗi có đang hiển thị hay không.
    /// </summary>
    public bool CoThongBaoLoiHienThi()
    {
        return CoHienThi(hopThongBaoLoi);
    }

    /// <summary>
    /// Lấy nội dung văn bản của thông báo lỗi khi đăng nhập thất bại.
    /// </summary>
    public string LayThongBaoLoi()
    {
        return Tim(hopThongBaoLoi).Text;
    }
}
