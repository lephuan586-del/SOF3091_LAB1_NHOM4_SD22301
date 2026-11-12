using FirstTests.Pages;
using NUnit.Framework;

namespace FirstTests.Tests;

[TestFixture]
public class SortTests : BaseTest
{
    [Test]
    public void SortProducts_PriceLowToHigh_PricesAreSortedAscending()
    {
        // 1. Chuỗi hành vi liên hoàn (method chaining) từ đăng nhập đến sắp xếp và lấy danh sách giá
        var actualPrices = new LoginPage(driver)
            .MoTrang()
            .DangNhap("standard_user", "secret_sauce")
            .SapXep("lohi")
            .LayDanhSachGia();

        // 2. In thông tin giá để kiểm chứng
        TestContext.Out.WriteLine("Danh sách giá sau khi sắp xếp Low to High: " + string.Join(", ", actualPrices));

        // 3. Assert số lượng phải đúng 6 sản phẩm
        Assert.That(actualPrices.Count, Is.EqualTo(6), "Số lượng sản phẩm hiển thị trên trang phải là 6.");

        // 4. Assert danh sách giá thực sự tăng dần
        Assert.That(actualPrices, Is.Ordered.Ascending, "Danh sách giá phải được sắp xếp tăng dần từ thấp đến cao.");

        var expectedSortedPrices = new List<decimal>(actualPrices);
        expectedSortedPrices.Sort();
        Assert.That(actualPrices, Is.EqualTo(expectedSortedPrices), "Thứ tự giá thực tế phải khớp hoàn toàn với danh sách sắp xếp tăng dần.");
    }
}
