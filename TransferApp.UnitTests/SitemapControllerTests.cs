using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TransferApp.Controllers;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class SitemapControllerTests
{
    [Test]
    public void Sitemap_Returns_Xml_With_Links()
    {
        var c = new SitemapController();
        c.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        c.Request.Scheme = "https";
        c.Request.Host = new HostString("example.com");

        var result = c.Sitemap();

        Assert.That(result, Is.InstanceOf<ContentResult>());
        var content = (ContentResult)result;
        Assert.That(content.ContentType, Is.EqualTo("application/xml"));
        Assert.That(content.Content, Does.Contain("https://example.com/"));
        Assert.That(content.Content, Does.Contain("/Pages/About"));
        Assert.That(content.Content, Does.Contain("/Pages/Prices"));
        Assert.That(content.Content, Does.Contain("/Pages/Contacts"));
    }
}
