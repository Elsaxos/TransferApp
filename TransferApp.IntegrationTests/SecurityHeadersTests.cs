using NUnit.Framework;
using System.Net;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class SecurityHeadersTests
{
    [Test]
    public async Task Home_Page_Should_Return_Security_Headers()
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync("/");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Headers.Contains("X-Content-Type-Options"), Is.True);
        Assert.That(response.Headers.Contains("X-Frame-Options"), Is.True);
        Assert.That(response.Headers.Contains("Referrer-Policy"), Is.True);
    }
}
