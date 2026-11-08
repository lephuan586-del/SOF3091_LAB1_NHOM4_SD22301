using System.Globalization;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests;

[TestFixture]
public class SortTests : BaseTest
{
    private void LoginAsStandardUser()
    {
        // Điều hướng đến saucedemo
        driver.Navigate().GoToUrl("https://www.saucedemo.com");

        // Đăng nhập bằng standard_user / secret_sauce
        var usernameInput = wait.Until(d => d.FindElement(By.Id("user-name")));
        usernameInput.SendKeys("standard_user");

        driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
        driver.FindElement(By.Id("login-button")).Click();

        // Đợi chuyển trang thành công đến inventory
        wait.Until(d => d.Url.Contains("inventory.html"));
    }

    [Test]
    public void SortProducts_ByPriceLowToHigh_PricesAreSortedAscending()
    {
        // 1. Đăng nhập hệ thống
        LoginAsStandardUser();

        // 2. Tìm dropdown sắp xếp và sử dụng SelectElement để chọn "Price (low to high)"
        var sortDropdownElement = wait.Until(d => d.FindElement(By.CssSelector("[data-test='product-sort-container']")));
        var selectElement = new SelectElement(sortDropdownElement);
        selectElement.SelectByText("Price (low to high)");

        // 3. Lấy danh sách các phần tử giá sản phẩm (.inventory_item_price)
        var priceElements = wait.Until(d =>
        {
            var elements = d.FindElements(By.CssSelector(".inventory_item_price"));
            return elements.Count > 0 ? elements : null;
        });

        // 4. Bỏ ký tự '$' và chuyển đổi sang kiểu decimal bằng CultureInfo.InvariantCulture
        var actualPrices = priceElements
            .Select(el => decimal.Parse(el.Text.Replace("$", string.Empty).Trim(), CultureInfo.InvariantCulture))
            .ToList();

        // 5. In ra thông tin giá để kiểm chứng
        TestContext.Out.WriteLine("Danh sách giá sau khi sắp xếp Low to High: " + string.Join(", ", actualPrices));

        // 6. Assert số lượng phải đúng 6 sản phẩm
        Assert.That(actualPrices.Count, Is.EqualTo(6), "Số lượng sản phẩm hiển thị trên trang phải là 6.");

        // 7. Assert danh sách giá thực sự tăng dần
        Assert.That(actualPrices, Is.Ordered.Ascending, "Danh sách giá phải được sắp xếp tăng dần từ thấp đến cao.");

        var expectedSortedPrices = actualPrices.OrderBy(p => p).ToList();
        Assert.That(actualPrices, Is.EqualTo(expectedSortedPrices), "Thứ tự giá thực tế phải khớp hoàn toàn với danh sách sắp xếp tăng dần.");
    }
}
