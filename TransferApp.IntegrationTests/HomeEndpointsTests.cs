using System.Net;
using NUnit.Framework;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class HomeEndpointsTests
{
    [TestCase("/")]
    [TestCase("/Home/Privacy")]
    public async Task Home_Pages_Should_Return_200(string url)
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync(url);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
