using NUnit.Framework;
using OpenQA.Selenium;

namespace FirstTests.Tests;

[TestFixture]
public class DemoFailTests : BaseTest
{
    [Test]
    [Category("Demo")]
    public void PartD_CoY_Rot_DeChupAnh()
    {
        // 1. Äiá»u hÆ°á»›ng Ä‘áº¿n trang javascript_alerts
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");

        // 2. KÃ­ch hoáº¡t JS Alert vÃ  Accept
        wait.Until(d => d.FindElement(By.CssSelector("button[onclick='jsAlert()']"))).Click();
        var alert = wait.Until(d => d.SwitchTo().Alert());
        alert.Accept();

        // 3. Láº¥y pháº§n tá»­ káº¿t quáº£ hiá»ƒn thá»‹
        var resultElement = wait.Until(d => d.FindElement(By.Id("result")));

        // 4. Cá» Ã ASSERT SAI: Káº¿t quáº£ thá»±c táº¿ lÃ  "You successfully clicked an alert"
        // nhÆ°ng cá»‘ Ã½ so sÃ¡nh vá»›i chuá»—i sai Ä‘á»ƒ test bá»‹ Fail vÃ  kÃ­ch hoáº¡t cÆ¡ cháº¿ chá»¥p áº£nh mÃ n hÃ¬nh cá»§a BaseTest
        Assert.That(resultElement.Text, Is.EqualTo("Chuá»—i nÃ y cá»‘ Ã½ sai Ä‘á»ƒ kiá»ƒm thá»­ chá»©c nÄƒng tá»± Ä‘á»™ng chá»¥p áº£nh mÃ n hÃ¬nh khi test tháº¥t báº¡i"),
            "Cá»‘ Ã½ gÃ¢y lá»—i Assertion Ä‘á»ƒ kiá»ƒm chá»©ng BaseTest.TearDown tá»± Ä‘á»™ng chá»¥p mÃ n hÃ¬nh vÃ  lÆ°u vÃ o thÆ° má»¥c screenshots.");
    }
}
