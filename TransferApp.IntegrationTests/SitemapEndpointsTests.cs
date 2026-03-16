using NUnit.Framework;
using System.Net;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class SitemapEndpointsTests
{
    [Test]
    public async Task Sitemap_Should_Return_Xml()
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync("/sitemap.xml");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType?.MediaType, Is.EqualTo("text/xml"));

        var xml = await response.Content.ReadAsStringAsync();
        Assert.That(xml, Does.Contain("<urlset"));
        Assert.That(xml, Does.Contain("YOURDOMAIN.COM").Or.Contain("https://localhost"));
    }
}
