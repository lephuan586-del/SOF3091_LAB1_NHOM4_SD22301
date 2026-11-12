using FirstTests.Pages;
using NUnit.Framework;

namespace FirstTests.Tests;

[TestFixture]
public class AlertIframeTests : BaseTest
{
    #region JavaScript Alerts Tests

    [Test]
    public void PartC_JsAlert_AcceptAndAssertResult()
    {
        var alertsPage = new JavaScriptAlertsPage(driver);
        alertsPage.MoTrang().BamNutAlert();

        // Kiểm tra nội dung alert trước khi Accept
        string noiDungAlert = alertsPage.LayNoiDungAlert();
        Assert.That(noiDungAlert, Is.EqualTo("I am a JS Alert"), "Nội dung hộp thoại Alert không đúng.");

        alertsPage.ChapNhanAlert();

        // Kiểm tra kết quả hiển thị trên trang
        Assert.That(alertsPage.LayKetQua(), Is.EqualTo("You successfully clicked an alert"), "Kết quả sau khi Accept Alert không đúng.");
    }

    [Test]
    public void PartC_JsConfirm_DismissAndAssertResult()
    {
        var alertsPage = new JavaScriptAlertsPage(driver);
        alertsPage.MoTrang().BamNutConfirm();

        // Kiểm tra nội dung alert trước khi Dismiss
        string noiDungConfirm = alertsPage.LayNoiDungAlert();
        Assert.That(noiDungConfirm, Is.EqualTo("I am a JS Confirm"), "Nội dung hộp thoại Confirm không đúng.");

        alertsPage.HuyAlert();

        // Kiểm tra kết quả hiển thị sau khi hủy
        Assert.That(alertsPage.LayKetQua(), Is.EqualTo("You clicked: Cancel"), "Kết quả sau khi Dismiss Confirm không đúng.");
    }

    [Test]
    public void PartC_JsPrompt_SendKeysAcceptAndAssertResult()
    {
        var alertsPage = new JavaScriptAlertsPage(driver);
        alertsPage.MoTrang().BamNutPrompt();

        // Kiểm tra nội dung prompt trước khi nhập
        string noiDungPrompt = alertsPage.LayNoiDungAlert();
        Assert.That(noiDungPrompt, Is.EqualTo("I am a JS prompt"), "Nội dung hộp thoại Prompt không đúng.");

        string inputText = "Selenium C# Lab 3";
        alertsPage.NhapVaChapNhanPrompt(inputText);

        // Kiểm tra kết quả hiển thị
        Assert.That(alertsPage.LayKetQua(), Is.EqualTo($"You entered: {inputText}"), "Kết quả sau khi nhập Prompt không đúng.");
    }

    #endregion

    #region iFrame Tests

    [Test]
    public void PartC_TinyMceIframe_WriteTextAndSwitchBack()
    {
        var iframePage = new IframePage(driver);
        iframePage.MoTrang();

        string textToEnter = "Kiem thu tu dong voi Selenium WebDriver trong iFrame.";
        iframePage.GoVaoEditor(textToEnter);

        // Kiểm tra nội dung bên trong trình soạn thảo editor
        Assert.That(iframePage.LayNoiDungEditor(), Does.Contain(textToEnter), "Nội dung trong trình soạn thảo TinyMCE không đúng với nội dung đã nhập.");

        // Kiểm tra tiêu đề trên trang chính sau khi đã thoát iframe
        Assert.That(iframePage.LayTieuDeTrangChinh(), Does.Contain("An iFrame containing the TinyMCE WYSIWYG Editor"));
    }

    #endregion
}
