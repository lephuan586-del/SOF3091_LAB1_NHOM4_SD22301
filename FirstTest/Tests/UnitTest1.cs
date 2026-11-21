using FirstTests.Core;
using FirstTests.Pages;
using NUnit.Framework;

namespace FirstTests.Tests;

[TestFixture]
public class Tests : BaseTest
{
    protected override string StartUrl => Config.ExampleUrl;

    [Test]
    public void OpenExampleSite_TitleIsCorrect()
    {
        var examplePage = new ExamplePage(Driver);
        examplePage.MoTrang();

        string title = examplePage.LayTieuDeTrang();
        TestContext.Out.WriteLine("Tiêu đề trang: " + title);

        Assert.That(title, Is.EqualTo("Example Domain"));
    }
}
