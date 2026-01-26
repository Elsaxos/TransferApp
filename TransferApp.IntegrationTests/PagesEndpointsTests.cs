using System.Net;
using NUnit.Framework;

namespace TransferApp.IntegrationTests;

[TestFixture]
public class PagesEndpointsTests
{
    [TestCase("/")]
    [TestCase("/Home/Index")]
    [TestCase("/Home/Privacy")]
    [TestCase("/Pages/About")]
    [TestCase("/Pages/Prices")]
    [TestCase("/Pages/Contacts")]
    public async Task Public_Pages_Should_Return_200(string url)
    {
        await using var factory = new CustomWebApplicationFactory(authenticate: false);
        using var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = true
        });

        var response = await client.GetAsync(url);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var html = await response.Content.ReadAsStringAsync();
        Assert.That(string.IsNullOrWhiteSpace(html), Is.False);
    }

    [Test]
    public async Task Prices_Page_Should_Render_Some_Content()
    {
        await using var factory = new CustomWebApplicationFactory(authenticate: false);
        using var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = true
        });

        var response = await client.GetAsync("/Pages/Prices");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var html = await response.Content.ReadAsStringAsync();
        Assert.That(string.IsNullOrWhiteSpace(html), Is.False);

        // по-нежна проверка – ако смениш текста, тестът пак ще е стабилен
        Assert.That(html, Does.Contain("price").IgnoreCase.Or.Contain("цена").IgnoreCase.Or.Contain("PROTRANSFER").IgnoreCase);
    }

    [Test]
    public async Task Contacts_Page_Should_Render_Some_Content()
    {
        await using var factory = new CustomWebApplicationFactory(authenticate: false);
        using var client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = true
        });

        var response = await client.GetAsync("/Pages/Contacts");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var html = await response.Content.ReadAsStringAsync();
        Assert.That(string.IsNullOrWhiteSpace(html), Is.False);
        Assert.That(html, Does.Contain("contact").IgnoreCase.Or.Contain("контакт").IgnoreCase);
    }
}



