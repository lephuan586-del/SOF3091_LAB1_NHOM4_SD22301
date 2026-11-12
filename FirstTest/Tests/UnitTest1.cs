using FirstTests.Pages;
using NUnit.Framework;

namespace FirstTests.Tests;

[TestFixture]
public class Tests : BaseTest
{
    [Test]
    public void OpenExampleSite_TitleIsCorrect()
    {
        var examplePage = new ExamplePage(driver);
        examplePage.MoTrang();

        string title = examplePage.LayTieuDeTrang();
        TestContext.Out.WriteLine("Tiêu đề trang: " + title);

        Assert.That(title, Is.EqualTo("Example Domain"));
    }
}
