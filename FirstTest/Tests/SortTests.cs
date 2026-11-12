using System.Globalization;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Tests;

[TestFixture]
public class SortTests : BaseTest
{
    private void LoginAsStandardUser()
    {
        // Äiá»u hÆ°á»›ng Ä‘áº¿n saucedemo
        driver.Navigate().GoToUrl("https://www.saucedemo.com");

        // ÄÄƒng nháº­p báº±ng standard_user / secret_sauce
        var usernameInput = wait.Until(d => d.FindElement(By.Id("user-name")));
        usernameInput.SendKeys("standard_user");

        driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
        driver.FindElement(By.Id("login-button")).Click();

        // Äá»£i chuyá»ƒn trang thÃ nh cÃ´ng Ä‘áº¿n inventory
        wait.Until(d => d.Url.Contains("inventory.html"));
    }

    [Test]
    public void SortProducts_ByPriceLowToHigh_PricesAreSortedAscending()
    {
        // 1. ÄÄƒng nháº­p há»‡ thá»‘ng
        LoginAsStandardUser();

        // 2. TÃ¬m dropdown sáº¯p xáº¿p vÃ  sá»­ dá»¥ng SelectElement Ä‘á»ƒ chá»n "Price (low to high)"
        var sortDropdownElement = wait.Until(d => d.FindElement(By.CssSelector("[data-test='product-sort-container']")));
        var selectElement = new SelectElement(sortDropdownElement);
        selectElement.SelectByText("Price (low to high)");

        // 3. Láº¥y danh sÃ¡ch cÃ¡c pháº§n tá»­ giÃ¡ sáº£n pháº©m (.inventory_item_price)
        var priceElements = wait.Until(d =>
        {
            var elements = d.FindElements(By.CssSelector(".inventory_item_price"));
            return elements.Count > 0 ? elements : null;
        });

        // 4. Bá» kÃ½ tá»± '$' vÃ  chuyá»ƒn Ä‘á»•i sang kiá»ƒu decimal báº±ng CultureInfo.InvariantCulture
        var actualPrices = priceElements
            .Select(el => decimal.Parse(el.Text.Replace("$", string.Empty).Trim(), CultureInfo.InvariantCulture))
            .ToList();

        // 5. In ra thÃ´ng tin giÃ¡ Ä‘á»ƒ kiá»ƒm chá»©ng
        TestContext.Out.WriteLine("Danh sÃ¡ch giÃ¡ sau khi sáº¯p xáº¿p Low to High: " + string.Join(", ", actualPrices));

        // 6. Assert sá»‘ lÆ°á»£ng pháº£i Ä‘Ãºng 6 sáº£n pháº©m
        Assert.That(actualPrices.Count, Is.EqualTo(6), "Sá»‘ lÆ°á»£ng sáº£n pháº©m hiá»ƒn thá»‹ trÃªn trang pháº£i lÃ  6.");

        // 7. Assert danh sÃ¡ch giÃ¡ thá»±c sá»± tÄƒng dáº§n
        Assert.That(actualPrices, Is.Ordered.Ascending, "Danh sÃ¡ch giÃ¡ pháº£i Ä‘Æ°á»£c sáº¯p xáº¿p tÄƒng dáº§n tá»« tháº¥p Ä‘áº¿n cao.");

        var expectedSortedPrices = actualPrices.OrderBy(p => p).ToList();
        Assert.That(actualPrices, Is.EqualTo(expectedSortedPrices), "Thá»© tá»± giÃ¡ thá»±c táº¿ pháº£i khá»›p hoÃ n toÃ n vá»›i danh sÃ¡ch sáº¯p xáº¿p tÄƒng dáº§n.");
    }
}
