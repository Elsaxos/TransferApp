using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System;
using System.Net;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class AdminAuthorizationTests
{
    [TestCase("/Admin/Inquiries")]
    [TestCase("/Admin/Reservations")]
    [TestCase("/Admin/Prices")]
    public async Task Get_Admin_Endpoints_Anonymous_ShouldRedirectToLogin(string url)
    {
        await using var factory = new CustomWebApplicationFactory();



        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync(url);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));

        var location = response.Headers.Location;
        Assert.That(location, Is.Not.Null, "Redirect should contain Location header");

      
     
        //  - https://localhost/Account/Login?ReturnUrl=...
        var loginPath = location!.IsAbsoluteUri ? location.AbsolutePath : location.OriginalString;

        Assert.That(loginPath, Does.StartWith("/Account/Login"));

        var locationText = location.ToString();
        Assert.That(locationText, Does.Contain("ReturnUrl="));
    }
}


