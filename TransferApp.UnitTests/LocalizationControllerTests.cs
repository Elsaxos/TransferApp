using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TransferApp.Controllers;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class LocalizationControllerTests
{
    [Test]
    public void SetLanguage_Sets_Culture_Cookie_And_Redirects()
    {
        var c = new LocalizationController();
        c.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = c.SetLanguage("en", "/Pages/About");

        Assert.That(result, Is.InstanceOf<LocalRedirectResult>());
        var redirect = (LocalRedirectResult)result;
        Assert.That(redirect.Url, Is.EqualTo("/Pages/About"));

        var header = c.Response.Headers["Set-Cookie"].ToString();
        var decoded = System.Net.WebUtility.UrlDecode(header);
        Assert.That(header, Does.Contain(CookieRequestCultureProvider.DefaultCookieName));
        Assert.That(decoded, Does.Contain("c=en"));
        Assert.That(decoded, Does.Contain("uic=en"));
    }
}
