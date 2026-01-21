
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class PagesEndpointsTests
{
    [TestCase("/Pages/About")]
    [TestCase("/Pages/Prices")]
    [TestCase("/Pages/Contacts")]
    public async Task Public_Pages_Should_Return_200(string url)
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync(url);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Prices_Page_Should_Render_Some_Content()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/Pages/Prices");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var html = await response.Content.ReadAsStringAsync();
        Assert.That(string.IsNullOrWhiteSpace(html), Is.False);

        
        Assert.That(html, Does.Contain("PROTRANSFER").IgnoreCase);
    }
}


