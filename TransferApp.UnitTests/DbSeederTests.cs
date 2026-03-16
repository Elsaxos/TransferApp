using NUnit.Framework;
using TransferApp.Data;
using TransferApp.UnitTests.TestHelpers;

namespace TransferApp.UnitTests.Data;

[TestFixture]
public class DbSeederTests
{
    [Test]
    public void SeedPriceItems_Adds_Defaults_When_Empty()
    {
        using var db = TestDb.CreateContext(nameof(SeedPriceItems_Adds_Defaults_When_Empty));

        DbSeeder.SeedPriceItems(db);

        Assert.That(db.PriceItems.Count(), Is.GreaterThan(0));
    }

    [Test]
    public void SeedPriceItems_Does_Not_Duplicate_When_Already_Seeded()
    {
        using var db = TestDb.CreateContext(nameof(SeedPriceItems_Does_Not_Duplicate_When_Already_Seeded));

        DbSeeder.SeedPriceItems(db);
        var count = db.PriceItems.Count();

        DbSeeder.SeedPriceItems(db);
        var countAfter = db.PriceItems.Count();

        Assert.That(countAfter, Is.EqualTo(count));
    }
}
