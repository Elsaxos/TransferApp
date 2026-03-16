using NUnit.Framework;
using System.Net;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class LocalizationEndpointsTests
{
    [Test]
    public async Task SetLanguage_Should_Set_Culture_Cookie()
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync("/Localization/SetLanguage?culture=fr&returnUrl=/");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
        Assert.That(response.Headers.Contains("Set-Cookie"), Is.True);
    }
}
