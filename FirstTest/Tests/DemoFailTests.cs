using FirstTests.Core;
using FirstTests.Pages;
using NUnit.Framework;

namespace FirstTests.Tests;

[TestFixture]
public class DemoFailTests : BaseTest
{
    protected override string StartUrl => Config.TheInternetUrl;

    [Test]
    [Category("Demo")]
    public void PartD_CoY_Rot_DeChupAnh()
    {
        var alertsPage = new JavaScriptAlertsPage(Driver);
        alertsPage.MoTrang().BamNutAlert().ChapNhanAlert();

        string ketQua = alertsPage.LayKetQua();

        // Cố ý so sánh sai để kích hoạt cơ chế chụp ảnh màn hình của BaseTest.TearDown
        Assert.That(ketQua, Is.EqualTo("Chuỗi này cố ý sai để kiểm thử chức năng tự động chụp ảnh màn hình khi test thất bại"),
            "Cố ý gây lỗi Assertion để kiểm chứng BaseTest.TearDown tự động chụp màn hình và lưu vào thư mục screenshots.");
    }
}
