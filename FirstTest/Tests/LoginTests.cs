using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests.Tests;

public class LoginTests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        driver.Navigate().GoToUrl("https://www.saucedemo.com");
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    public void Login(string username, string password)
    {
        driver.FindElement(By.Id("user-name")).SendKeys(username);
        driver.FindElement(By.Id("password")).SendKeys(password);
        driver.FindElement(By.Id("login-button")).Click();
    }

    [Test]
    public void Login_ValidUser_GoesToInventoryPage()
    {
        Login("standard_user", "secret_sauce");
        Assert.That(driver.Url, Does.Contain("inventory.html"));
    }

    [Test]
    public void Login_LockedOutUser_ShowsErrorMessage()
    {
        Login("locked_out_user", "secret_sauce");

        var errorMessage = driver.FindElement(By.CssSelector("[data-test='error']"));

        Assert.That(errorMessage.Displayed, Is.True);
        Assert.That(errorMessage.Text, Is.EqualTo("Epic sadface: Sorry, this user has been locked out."));
        Assert.That(driver.Url, Does.Not.Contain("inventory.html"));
    }

    [Test]
    public void CollectProducts_PrintsAndAssertsDetails()
    {
        Login("standard_user", "secret_sauce");

        var productItems = driver.FindElements(By.CssSelector("[data-test='inventory-item']"));
        Assert.That(productItems.Count, Is.EqualTo(6), "Danh sÃ¡ch pháº£i cÃ³ Ä‘á»§ 6 sáº£n pháº©m.");

        var productNames = new List<string>();

        foreach (var item in productItems)
        {
            var nameElement = item.FindElement(By.CssSelector("[data-test='inventory-item-name']"));
            var priceElement = item.FindElement(By.CssSelector("[data-test='inventory-item-price']"));

            string name = nameElement.Text;
            string price = priceElement.Text;

            TestContext.Out.WriteLine($"Sáº£n pháº©m: {name} | GiÃ¡: {price}");

            Assert.That(string.IsNullOrWhiteSpace(name), Is.False, "TÃªn sáº£n pháº©m khÃ´ng Ä‘Æ°á»£c rá»—ng hoáº·c chá»‰ chá»©a khoáº£ng tráº¯ng.");
            Assert.That(price, Does.StartWith("$"), $"GiÃ¡ cá»§a sáº£n pháº©m '{name}' pháº£i báº¯t Ä‘áº§u báº±ng '$'.");

            productNames.Add(name);
        }

        Assert.That(productNames, Does.Contain("Sauce Labs Backpack"), "Danh sÃ¡ch sáº£n pháº©m pháº£i chá»©a 'Sauce Labs Backpack'.");
    }
}
