using NUnit.Framework;
using OpenQA.Selenium;

namespace FirstTests;

[TestFixture]
public class AlertIframeTests : BaseTest
{
    private const string AlertsUrl = "https://the-internet.herokuapp.com/javascript_alerts";
    private const string IframeUrl = "https://the-internet.herokuapp.com/iframe";

    #region JavaScript Alerts Tests

    [Test]
    public void PartC_JsAlert_AcceptAndAssertResult()
    {
        // 1. Mở trang JS Alerts
        driver.Navigate().GoToUrl(AlertsUrl);

        // 2. Bấm nút kích hoạt JS Alert
        wait.Until(d => d.FindElement(By.CssSelector("button[onclick='jsAlert()']"))).Click();

        // 3. Đợi alert xuất hiện và kiểm tra text trước khi Accept
        var alert = wait.Until(d => d.SwitchTo().Alert());
        Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"), "Nội dung hộp thoại Alert không đúng.");

        // 4. Accept alert
        alert.Accept();

        // 5. Assert nội dung kết quả trên trang
        var resultElement = wait.Until(d => d.FindElement(By.Id("result")));
        Assert.That(resultElement.Text, Is.EqualTo("You successfully clicked an alert"), "Kết quả sau khi Accept Alert không đúng.");
    }

    [Test]
    public void PartC_JsConfirm_DismissAndAssertResult()
    {
        // 1. Mở trang JS Alerts
        driver.Navigate().GoToUrl(AlertsUrl);

        // 2. Bấm nút kích hoạt JS Confirm
        wait.Until(d => d.FindElement(By.CssSelector("button[onclick='jsConfirm()']"))).Click();

        // 3. Đợi alert xuất hiện và kiểm tra text trước khi Dismiss
        var alert = wait.Until(d => d.SwitchTo().Alert());
        Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"), "Nội dung hộp thoại Confirm không đúng.");

        // 4. Dismiss (Cancel) alert
        alert.Dismiss();

        // 5. Assert nội dung kết quả trên trang
        var resultElement = wait.Until(d => d.FindElement(By.Id("result")));
        Assert.That(resultElement.Text, Is.EqualTo("You clicked: Cancel"), "Kết quả sau khi Dismiss Confirm không đúng.");
    }

    [Test]
    public void PartC_JsPrompt_SendKeysAcceptAndAssertResult()
    {
        // 1. Mở trang JS Alerts
        driver.Navigate().GoToUrl(AlertsUrl);

        // 2. Bấm nút kích hoạt JS Prompt
        wait.Until(d => d.FindElement(By.CssSelector("button[onclick='jsPrompt()']"))).Click();

        // 3. Đợi alert xuất hiện và kiểm tra text
        var alert = wait.Until(d => d.SwitchTo().Alert());
        Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"), "Nội dung hộp thoại Prompt không đúng.");

        // 4. Nhập chuỗi vào prompt và Accept
        string inputText = "Selenium C# Lab 3";
        alert.SendKeys(inputText);
        alert.Accept();

        // 5. Assert nội dung kết quả trên trang chứa đúng chuỗi vừa nhập
        var resultElement = wait.Until(d => d.FindElement(By.Id("result")));
        Assert.That(resultElement.Text, Is.EqualTo($"You entered: {inputText}"), "Kết quả sau khi nhập Prompt không đúng.");
    }

    #endregion

    #region iFrame Tests

    [Test]
    public void PartC_TinyMceIframe_WriteTextAndSwitchBack()
    {
        // 1. Mở trang iFrame
        driver.Navigate().GoToUrl(IframeUrl);

        // 2. Xử lý banner thông báo của TinyMCE nếu có cản trở giao diện
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
            // Bỏ qua nếu không có thông báo hoặc đã tự tắt
        }

        // 3. Đợi iframe editor xuất hiện
        wait.Until(d => d.FindElement(By.Id("mce_0_ifr")));

        try
        {
            // 4. Chuyển ngữ cảnh vào bên trong iFrame
            driver.SwitchTo().Frame("mce_0_ifr");

            // 5. Tìm phần tử soạn thảo (thẻ body với id='tinymce')
            var editorBody = wait.Until(d => d.FindElement(By.Id("tinymce")));

            // Focus vào editor và xóa nội dung cũ
            editorBody.Click();
            editorBody.Clear();

            // Nhập đoạn văn bản mới
            string textToEnter = "Kiem thu tu dong voi Selenium WebDriver trong iFrame.";
            editorBody.SendKeys(textToEnter);

            // Đảm bảo nội dung đã xuất hiện trong editor
            wait.Until(d => editorBody.Text.Contains(textToEnter));

            // 6. Assert nội dung đã nhập vào editor
            Assert.That(editorBody.Text, Does.Contain(textToEnter), "Nội dung trong trình soạn thảo TinyMCE không đúng với nội dung đã nhập.");
        }
        finally
        {
            // 7. BẮT BUỘC: Luôn chuyển lại ngữ cảnh trang chính trong khối finally
            driver.SwitchTo().DefaultContent();
        }

        // 8. Xác nhận đã trở về trang chính an toàn (tìm phần tử h3 bên ngoài iframe)
        var heading = wait.Until(d => d.FindElement(By.TagName("h3")));
        Assert.That(heading.Text, Does.Contain("An iFrame containing the TinyMCE WYSIWYG Editor"));
    }

    #endregion
}
