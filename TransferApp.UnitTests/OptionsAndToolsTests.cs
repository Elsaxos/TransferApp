using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using TransferApp.Options;
using TransferApp.Tools;

namespace TransferApp.UnitTests;

[TestFixture]
public class OptionsAndToolsTests
{
    [Test]
    public void AdminHashGenerator_Produces_Verifiable_Hash()
    {
        var hash = AdminHashGenerator.Hash("admin", "12@12@34");
        Assert.That(string.IsNullOrWhiteSpace(hash), Is.False);

        var hasher = new PasswordHasher<AdminUser>();
        var result = hasher.VerifyHashedPassword(new AdminUser { Username = "admin" }, hash, "12@12@34");
        Assert.That(result, Is.Not.EqualTo(PasswordVerificationResult.Failed));
    }

    [Test]
    public void AdminUsersOptions_Defaults_To_Empty_List()
    {
        var opts = new AdminUsersOptions();
        Assert.That(opts.Users, Is.Not.Null);
        Assert.That(opts.Users.Count, Is.EqualTo(0));
    }
}
