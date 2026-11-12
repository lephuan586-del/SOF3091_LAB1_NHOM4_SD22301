using NUnit.Framework;
using OpenQA.Selenium;

namespace FirstTests.Tests;

[TestFixture]
public class WaitTests : BaseTest
{
    [Test]
    [Category("Demo")]
    public void PartA_KhongWait_Rot()
    {
        // 1. Äiá»u hÆ°á»›ng Ä‘áº¿n trang Dynamic Loading 2
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

        // 2. Báº¥m nÃºt Start Ä‘á»ƒ kÃ­ch hoáº¡t táº£i dá»¯ liá»‡u báº±ng JavaScript
        driver.FindElement(By.CssSelector("#start button")).Click();

        // 3. KHÃ”NG CHá»œ: TÃ¬m ngay láº­p tá»©c pháº§n tá»­ #finish khi DOM chÆ°a ká»‹p render -> GÃ¢y lá»—i NoSuchElementException
        var finishElement = driver.FindElement(By.Id("finish"));

        // 4. Kiá»ƒm tra ná»™i dung vÄƒn báº£n
        Assert.That(finishElement.Text, Does.Contain("Hello World!"));
    }

    [Test]
    public void PartA_CoWait_Pass()
    {
        // 1. Äiá»u hÆ°á»›ng Ä‘áº¿n trang Dynamic Loading 2
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

        // 2. Báº¥m nÃºt Start Ä‘á»ƒ kÃ­ch hoáº¡t táº£i dá»¯ liá»‡u báº±ng JavaScript
        driver.FindElement(By.CssSelector("#start button")).Click();

        // 3. CÃ“ CHá»œ: DÃ¹ng WebDriverWait poll liÃªn tá»¥c Ä‘áº¿n khi #finish xuáº¥t hiá»‡n trong DOM (timeout 10 giÃ¢y)
        var finishElement = wait.Until(d => d.FindElement(By.Id("finish")));

        // 4. Kiá»ƒm tra ná»™i dung vÄƒn báº£n
        Assert.That(finishElement.Text, Does.Contain("Hello World!"));
    }
}
