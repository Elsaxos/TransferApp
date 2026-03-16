using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TransferApp.Data;
using TransferApp.Models;

namespace TransferApp.UnitTests.Data;

[TestFixture]
public class ApplicationDbContextTests
{
    [Test]
    public void Model_Has_Precision_For_Prices()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(nameof(Model_Has_Precision_For_Prices))
            .Options;

        using var db = new ApplicationDbContext(options);
        var priceItem = db.Model.FindEntityType(typeof(PriceItem));
        var transfer = db.Model.FindEntityType(typeof(TransferRequest));

        Assert.That(priceItem, Is.Not.Null);
        Assert.That(transfer, Is.Not.Null);

        var oneWay = priceItem!.FindProperty(nameof(PriceItem.OneWayPrice));
        var roundTrip = priceItem!.FindProperty(nameof(PriceItem.RoundTripPrice));
        var price = transfer!.FindProperty(nameof(TransferRequest.Price));

        Assert.That(oneWay!.GetPrecision(), Is.EqualTo(18));
        Assert.That(oneWay!.GetScale(), Is.EqualTo(2));
        Assert.That(roundTrip!.GetPrecision(), Is.EqualTo(18));
        Assert.That(roundTrip!.GetScale(), Is.EqualTo(2));
        Assert.That(price!.GetPrecision(), Is.EqualTo(18));
        Assert.That(price!.GetScale(), Is.EqualTo(2));
    }
}
