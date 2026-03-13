using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using TransferApp.Controllers;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class HomeControllerTests
{
    [Test]
    public void Index_Returns_View()
    {
        var c = new HomeController(new LoggerFactory().CreateLogger<HomeController>());
        var result = c.Index();
        Assert.That(result, Is.InstanceOf<ViewResult>());
    }

    [Test]
    public void Error_Returns_View_With_RequestId()
    {
        var c = new HomeController(new LoggerFactory().CreateLogger<HomeController>());
        c.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = c.Error();

        Assert.That(result, Is.InstanceOf<ViewResult>());
        var vr = (ViewResult)result;
        Assert.That(vr.Model, Is.Not.Null);
    }

    [Test]
    public void SetLanguage_Sets_Culture_Cookie_And_Redirects()
    {
        var c = new HomeController(new LoggerFactory().CreateLogger<HomeController>());
        c.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = c.SetLanguage("fr", "/");

        Assert.That(result, Is.InstanceOf<LocalRedirectResult>());
        var header = c.Response.Headers["Set-Cookie"].ToString();
        var decoded = System.Net.WebUtility.UrlDecode(header);
        Assert.That(header, Does.Contain(CookieRequestCultureProvider.DefaultCookieName));
        Assert.That(decoded, Does.Contain("c=fr"));
        Assert.That(decoded, Does.Contain("uic=fr"));
    }
}
