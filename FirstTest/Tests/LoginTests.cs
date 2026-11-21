using System.Text.RegularExpressions;
using FirstTests.Core;
using FirstTests.Data;
using FirstTests.Pages;
using NUnit.Framework;

namespace FirstTests.Tests;

[TestFixture]
public class LoginTests : BaseTest
{
    private LoginPage LoginPage => new LoginPage(Driver);

    public static IEnumerable<TestCaseData> DuLieuDangNhap()
    {
        var danhSach = UserTestData.DocTuFileJson();
        for (int i = 0; i < danhSach.Count; i++)
        {
            var user = danhSach[i];
            string safeUsername = string.IsNullOrWhiteSpace(user.Username)
                ? "Rong"
                : Regex.Replace(user.Username, @"[^a-zA-Z0-9_]+", "_");

            if (safeUsername.Length > 20)
            {
                safeUsername = safeUsername.Substring(0, 20);
            }

            var testCase = new TestCaseData(user.Username, user.Password, user.ThanhCong, user.GhiChu)
                .SetName($"DangNhap_{i + 1:00}_{safeUsername}")
                .SetDescription(user.GhiChu);

            yield return testCase;
        }
    }

    [TestCaseSource(nameof(DuLieuDangNhap))]
    public void KiemThuDangNhap_DataDriven(string username, string password, bool thanhCong, string ghiChu)
    {
        var inventoryPage = LoginPage.MoTrang().DangNhap(username, password);

        Assert.That(inventoryPage.DaHienThi(), Is.EqualTo(thanhCong),
            $"Dang nhap voi Username: '{username}' | Ghi chu: '{ghiChu}' - Ket qua khong khop voi ky vong ({thanhCong}).");
    }

    [Test]
    public void CollectProducts_PrintsAndAssertsDetails()
    {
        var inventoryPage = LoginPage.MoTrang().DangNhap("standard_user", "secret_sauce");

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
