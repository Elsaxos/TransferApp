using System.Net;
using NUnit.Framework;

namespace TransferApp.IntegrationTests;

public class AdminAuthorizationTests
{
    [TestCase("/Admin/Inquiries")]
    [TestCase("/Admin/Reservations")]
    [TestCase("/Admin/Prices")]
    public async Task Get_Admin_Endpoints_Anonymous_ShouldRedirectToLogin(string url)
    {
        await using var factory = new CustomWebApplicationFactory(authenticate: false);

        using var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync(url);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));

        var location = response.Headers.Location;
        Assert.That(location, Is.Not.Null);

        // Location може да е absolute: https://localhost/Account/Login?ReturnUrl=...
        var pathAndQuery = location!.IsAbsoluteUri ? location.PathAndQuery : location.ToString();

        Assert.That(pathAndQuery, Does.StartWith("/Account/Login"));
    }

    [TestCase("/Admin/Inquiries")]
    [TestCase("/Admin/Reservations")]
    [TestCase("/Admin/Prices")]
    public async Task Get_Admin_Endpoints_Authenticated_ShouldReturn200(string url)
    {
        await using var factory = new CustomWebApplicationFactory(authenticate: true);
        using var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync(url);

        // Ако някъде има редирект/403, ще го хванем веднага
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var html = await response.Content.ReadAsStringAsync();
        Assert.That(string.IsNullOrWhiteSpace(html), Is.False);
    }
}



