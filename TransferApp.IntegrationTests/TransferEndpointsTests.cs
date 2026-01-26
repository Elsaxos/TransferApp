using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class TransferEndpointsTests
{
    [TestCase("/Transfer/Create")]
    [TestCase("/Transfer/Thanks")]
    public async Task Transfer_Pages_Should_Return_200_Or_Redirect(string url)
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync(url);
        
        Assert.That(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Redirect, Is.True);
    }
}
