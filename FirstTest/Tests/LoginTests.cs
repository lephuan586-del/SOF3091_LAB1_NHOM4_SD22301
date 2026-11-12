using FirstTests.Pages;
using NUnit.Framework;

namespace FirstTests.Tests;

[TestFixture]
public class LoginTests : BaseTest
{
    private LoginPage loginPage = null!;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        loginPage = new LoginPage(driver);
        loginPage.MoTrang();
    }

    [Test]
    public void Login_ValidUser_GoesToInventoryPage()
    {
        var inventoryPage = loginPage.DangNhap("standard_user", "secret_sauce");

        Assert.That(driver.Url, Does.Contain("inventory.html"));
    }

    [Test]
    public void Login_LockedOutUser_ShowsErrorMessage()
    {
        loginPage.DangNhapKhongThanhCong("locked_out_user", "secret_sauce");

        Assert.That(loginPage.CoThongBaoLoiHienThi(), Is.True);
        Assert.That(loginPage.LayThongBaoLoi(), Is.EqualTo("Epic sadface: Sorry, this user has been locked out."));
        Assert.That(driver.Url, Does.Not.Contain("inventory.html"));
    }

    [Test]
    public void CollectProducts_PrintsAndAssertsDetails()
    {
        var inventoryPage = loginPage.DangNhap("standard_user", "secret_sauce");

        var danhSachSanPham = inventoryPage.LayDanhSachSanPham();
        Assert.That(danhSachSanPham.Count, Is.EqualTo(6), "Danh sách phải có đủ 6 sản phẩm.");

        var productNames = new List<string>();

        foreach (var item in danhSachSanPham)
        {
            string name = item.Ten;
            string price = item.Gia;

            TestContext.Out.WriteLine($"Sản phẩm: {name} | Giá: {price}");

            Assert.That(string.IsNullOrWhiteSpace(name), Is.False, "Tên sản phẩm không được rỗng hoặc chỉ chứa khoảng trắng.");
            Assert.That(price, Does.StartWith("$"), $"Giá của sản phẩm '{name}' phải bắt đầu bằng '$'.");

            productNames.Add(name);
        }

        Assert.That(productNames, Does.Contain("Sauce Labs Backpack"), "Danh sách sản phẩm phải chứa 'Sauce Labs Backpack'.");
    }
}
