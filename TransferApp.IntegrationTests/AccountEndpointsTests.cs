using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class AccountEndpointsTests
{
    [Test]
    public async Task Login_Page_Should_Return_200()
    {
        await using var factory = new CustomWebApplicationFactory();

        using var client = factory.CreateClient();

        var response = await client.GetAsync("/Account/Login");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}

