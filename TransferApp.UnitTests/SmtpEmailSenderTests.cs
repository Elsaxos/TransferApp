using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TransferApp.Services;

namespace TransferApp.UnitTests.Services;

[TestFixture]
public class SmtpEmailSenderTests
{
    [Test]
    public void SendEmailAsync_Throws_When_Config_Missing()
    {
        var config = new ConfigurationBuilder().Build();
        var sender = new SmtpEmailSender(config);

        Assert.That(async () => await sender.SendEmailAsync("a@a.com", "s", "b"),
            Throws.InvalidOperationException.With.Message.Contains("Missing SMTP configuration"));
    }

    [Test]
    public void SendEmailAsync_Throws_When_Port_Invalid()
    {
        var dict = new Dictionary<string, string?>
        {
            ["Smtp:Host"] = "smtp.example.com",
            ["Smtp:Port"] = "abc",
            ["Smtp:User"] = "user",
            ["Smtp:Pass"] = "pass"
        };
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict!).Build();
        var sender = new SmtpEmailSender(config);

        Assert.That(async () => await sender.SendEmailAsync("a@a.com", "s", "b"),
            Throws.InstanceOf<FormatException>());
    }
}
