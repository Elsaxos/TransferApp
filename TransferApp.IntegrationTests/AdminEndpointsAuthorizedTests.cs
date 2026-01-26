using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class AdminEndpointsAuthorizedTests
{
    [TestCase("/Admin/Inquiries")]
    [TestCase("/Admin/Prices")]
    [TestCase("/Admin/Reservations")]
    public async Task Admin_Endpoints_Authorized_Should_Return_200(string url)
    {
        await using var factory = new CustomWebApplicationFactory(authenticate: true);

        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,           // ✅ важно
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.Found)
        {
            var location = response.Headers.Location?.ToString() ?? "";
            Assert.Fail($"Got 302 redirect to: {location}");
        }

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}

