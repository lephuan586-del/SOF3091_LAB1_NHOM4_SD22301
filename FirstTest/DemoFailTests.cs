using NUnit.Framework;
using OpenQA.Selenium;

namespace FirstTests;

[TestFixture]
public class DemoFailTests : BaseTest
{
    [Test]
    [Category("Demo")]
    public void PartD_CoY_Rot_DeChupAnh()
    {
        // 1. Điều hướng đến trang javascript_alerts
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");

        // 2. Kích hoạt JS Alert và Accept
        wait.Until(d => d.FindElement(By.CssSelector("button[onclick='jsAlert()']"))).Click();
        var alert = wait.Until(d => d.SwitchTo().Alert());
        alert.Accept();

        // 3. Lấy phần tử kết quả hiển thị
        var resultElement = wait.Until(d => d.FindElement(By.Id("result")));

        // 4. CỐ Ý ASSERT SAI: Kết quả thực tế là "You successfully clicked an alert"
        // nhưng cố ý so sánh với chuỗi sai để test bị Fail và kích hoạt cơ chế chụp ảnh màn hình của BaseTest
        Assert.That(resultElement.Text, Is.EqualTo("Chuỗi này cố ý sai để kiểm thử chức năng tự động chụp ảnh màn hình khi test thất bại"),
            "Cố ý gây lỗi Assertion để kiểm chứng BaseTest.TearDown tự động chụp màn hình và lưu vào thư mục screenshots.");
    }
}
