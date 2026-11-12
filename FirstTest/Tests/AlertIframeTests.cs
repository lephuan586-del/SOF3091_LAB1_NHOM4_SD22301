using NUnit.Framework;
using OpenQA.Selenium;

namespace FirstTests.Tests;

[TestFixture]
public class AlertIframeTests : BaseTest
{
    private const string AlertsUrl = "https://the-internet.herokuapp.com/javascript_alerts";
    private const string IframeUrl = "https://the-internet.herokuapp.com/iframe";

    #region JavaScript Alerts Tests

    [Test]
    public void PartC_JsAlert_AcceptAndAssertResult()
    {
        // 1. Má»Ÿ trang JS Alerts
        driver.Navigate().GoToUrl(AlertsUrl);

        // 2. Báº¥m nÃºt kÃ­ch hoáº¡t JS Alert
        wait.Until(d => d.FindElement(By.CssSelector("button[onclick='jsAlert()']"))).Click();

        // 3. Äá»£i alert xuáº¥t hiá»‡n vÃ  kiá»ƒm tra text trÆ°á»›c khi Accept
        var alert = wait.Until(d => d.SwitchTo().Alert());
        Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"), "Ná»™i dung há»™p thoáº¡i Alert khÃ´ng Ä‘Ãºng.");

        // 4. Accept alert
        alert.Accept();

        // 5. Assert ná»™i dung káº¿t quáº£ trÃªn trang
        var resultElement = wait.Until(d => d.FindElement(By.Id("result")));
        Assert.That(resultElement.Text, Is.EqualTo("You successfully clicked an alert"), "Káº¿t quáº£ sau khi Accept Alert khÃ´ng Ä‘Ãºng.");
    }

    [Test]
    public void PartC_JsConfirm_DismissAndAssertResult()
    {
        // 1. Má»Ÿ trang JS Alerts
        driver.Navigate().GoToUrl(AlertsUrl);

        // 2. Báº¥m nÃºt kÃ­ch hoáº¡t JS Confirm
        wait.Until(d => d.FindElement(By.CssSelector("button[onclick='jsConfirm()']"))).Click();

        // 3. Äá»£i alert xuáº¥t hiá»‡n vÃ  kiá»ƒm tra text trÆ°á»›c khi Dismiss
        var alert = wait.Until(d => d.SwitchTo().Alert());
        Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"), "Ná»™i dung há»™p thoáº¡i Confirm khÃ´ng Ä‘Ãºng.");

        // 4. Dismiss (Cancel) alert
        alert.Dismiss();

        // 5. Assert ná»™i dung káº¿t quáº£ trÃªn trang
        var resultElement = wait.Until(d => d.FindElement(By.Id("result")));
        Assert.That(resultElement.Text, Is.EqualTo("You clicked: Cancel"), "Káº¿t quáº£ sau khi Dismiss Confirm khÃ´ng Ä‘Ãºng.");
    }

    [Test]
    public void PartC_JsPrompt_SendKeysAcceptAndAssertResult()
    {
        // 1. Má»Ÿ trang JS Alerts
        driver.Navigate().GoToUrl(AlertsUrl);

        // 2. Báº¥m nÃºt kÃ­ch hoáº¡t JS Prompt
        wait.Until(d => d.FindElement(By.CssSelector("button[onclick='jsPrompt()']"))).Click();

        // 3. Äá»£i alert xuáº¥t hiá»‡n vÃ  kiá»ƒm tra text
        var alert = wait.Until(d => d.SwitchTo().Alert());
        Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"), "Ná»™i dung há»™p thoáº¡i Prompt khÃ´ng Ä‘Ãºng.");

        // 4. Nháº­p chuá»—i vÃ o prompt vÃ  Accept
        string inputText = "Selenium C# Lab 3";
        alert.SendKeys(inputText);
        alert.Accept();

        // 5. Assert ná»™i dung káº¿t quáº£ trÃªn trang chá»©a Ä‘Ãºng chuá»—i vá»«a nháº­p
        var resultElement = wait.Until(d => d.FindElement(By.Id("result")));
        Assert.That(resultElement.Text, Is.EqualTo($"You entered: {inputText}"), "Káº¿t quáº£ sau khi nháº­p Prompt khÃ´ng Ä‘Ãºng.");
    }

    #endregion

    #region iFrame Tests

    [Test]
    public void PartC_TinyMceIframe_WriteTextAndSwitchBack()
    {
        // 1. Má»Ÿ trang iFrame
        driver.Navigate().GoToUrl(IframeUrl);

        // 2. Xá»­ lÃ½ banner thÃ´ng bÃ¡o cá»§a TinyMCE náº¿u cÃ³ cáº£n trá»Ÿ giao diá»‡n
        try
        {
            var closeNotificationButtons = driver.FindElements(By.CssSelector(".tox-notification__dismiss-button, button[aria-label='Close']"));
            foreach (var btn in closeNotificationButtons)
            {
                if (btn.Displayed)
                {
                    btn.Click();
                }
            }
        }
        catch (WebDriverException)
        {
            // Bá» qua náº¿u khÃ´ng cÃ³ thÃ´ng bÃ¡o hoáº·c Ä‘Ã£ tá»± táº¯t
        }

        // 3. Äá»£i iframe editor xuáº¥t hiá»‡n
        wait.Until(d => d.FindElement(By.Id("mce_0_ifr")));

        try
        {
            // 4. Chuyá»ƒn ngá»¯ cáº£nh vÃ o bÃªn trong iFrame
            driver.SwitchTo().Frame("mce_0_ifr");

            // 5. TÃ¬m pháº§n tá»­ soáº¡n tháº£o (tháº» body vá»›i id='tinymce')
            var editorBody = wait.Until(d => d.FindElement(By.Id("tinymce")));

            // Focus vÃ o editor vÃ  xÃ³a ná»™i dung cÅ©
            editorBody.Click();
            editorBody.Clear();

            // Nháº­p Ä‘oáº¡n vÄƒn báº£n má»›i
            string textToEnter = "Kiem thu tu dong voi Selenium WebDriver trong iFrame.";
            editorBody.SendKeys(textToEnter);

            // Äáº£m báº£o ná»™i dung Ä‘Ã£ xuáº¥t hiá»‡n trong editor
            wait.Until(d => editorBody.Text.Contains(textToEnter));

            // 6. Assert ná»™i dung Ä‘Ã£ nháº­p vÃ o editor
            Assert.That(editorBody.Text, Does.Contain(textToEnter), "Ná»™i dung trong trÃ¬nh soáº¡n tháº£o TinyMCE khÃ´ng Ä‘Ãºng vá»›i ná»™i dung Ä‘Ã£ nháº­p.");
        }
        finally
        {
            // 7. Báº®T BUá»˜C: LuÃ´n chuyá»ƒn láº¡i ngá»¯ cáº£nh trang chÃ­nh trong khá»‘i finally
            driver.SwitchTo().DefaultContent();
        }

        // 8. XÃ¡c nháº­n Ä‘Ã£ trá»Ÿ vá» trang chÃ­nh an toÃ n (tÃ¬m pháº§n tá»­ h3 bÃªn ngoÃ i iframe)
        var heading = wait.Until(d => d.FindElement(By.TagName("h3")));
        Assert.That(heading.Text, Does.Contain("An iFrame containing the TinyMCE WYSIWYG Editor"));
    }

    #endregion
}
