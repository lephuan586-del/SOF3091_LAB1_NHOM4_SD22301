using FirstTests.Pages;
using NUnit.Framework;

namespace FirstTests.Tests;

[TestFixture]
public class WaitTests : BaseTest
{
    [Test]
    [Category("Demo")]
    public void PartA_KhongWait_Rot()
    {
        var dynamicLoadingPage = new DynamicLoadingPage(driver);
        dynamicLoadingPage.MoTrang().BamStart();

        // Không chờ: Đọc ngay lập tức khi DOM chưa kịp render -> Gây lỗi NoSuchElementException
        string ketQua = dynamicLoadingPage.LayKetQuaKhongCho();

        Assert.That(ketQua, Does.Contain("Hello World!"));
    }

    [Test]
    public void PartA_CoWait_Pass()
    {
        var dynamicLoadingPage = new DynamicLoadingPage(driver);
        dynamicLoadingPage.MoTrang().BamStart();

        // Có chờ: Explicit Wait đợi phần tử xuất hiện trong DOM
        string ketQua = dynamicLoadingPage.LayKetQua();

        Assert.That(ketQua, Does.Contain("Hello World!"));
    }
}
