using NUnit.Framework;
using OpenQA.Selenium;

namespace FirstTests;

[TestFixture]
public class WaitTests : BaseTest
{
    [Test]
    [Category("Demo")]
    public void PartA_KhongWait_Rot()
    {
        // 1. Điều hướng đến trang Dynamic Loading 2
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

        // 2. Bấm nút Start để kích hoạt tải dữ liệu bằng JavaScript
        driver.FindElement(By.CssSelector("#start button")).Click();

        // 3. KHÔNG CHỜ: Tìm ngay lập tức phần tử #finish khi DOM chưa kịp render -> Gây lỗi NoSuchElementException
        var finishElement = driver.FindElement(By.Id("finish"));

        // 4. Kiểm tra nội dung văn bản
        Assert.That(finishElement.Text, Does.Contain("Hello World!"));
    }

    [Test]
    public void PartA_CoWait_Pass()
    {
        // 1. Điều hướng đến trang Dynamic Loading 2
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

        // 2. Bấm nút Start để kích hoạt tải dữ liệu bằng JavaScript
        driver.FindElement(By.CssSelector("#start button")).Click();

        // 3. CÓ CHỜ: Dùng WebDriverWait poll liên tục đến khi #finish xuất hiện trong DOM (timeout 10 giây)
        var finishElement = wait.Until(d => d.FindElement(By.Id("finish")));

        // 4. Kiểm tra nội dung văn bản
        Assert.That(finishElement.Text, Does.Contain("Hello World!"));
    }
}
