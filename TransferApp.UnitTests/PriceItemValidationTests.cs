using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using TransferApp.Models;

namespace TransferApp.UnitTests;

[TestFixture]
public class PriceItemValidationTests
{
    [Test]
    public void RoundTripPrice_Less_Than_OneWay_Should_Fail_Validation()
    {
        var item = new PriceItem
        {
            RouteKey = "k",
            RouteBg = "bg",
            RouteEn = "en",
            RouteRu = "ru",
            RouteFr = "fr",
            OneWayPrice = 100,
            RoundTripPrice = 90,
            SortOrder = 1,
            IsActive = true
        };

        var ctx = new ValidationContext(item);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(item, ctx, results, validateAllProperties: true);

        Assert.That(valid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(PriceItem.RoundTripPrice))), Is.True);
    }

    [Test]
    public void RoundTripPrice_Greater_Or_Equal_OneWay_Should_Pass_Validation()
    {
        var item = new PriceItem
        {
            RouteKey = "k",
            RouteBg = "bg",
            RouteEn = "en",
            RouteRu = "ru",
            RouteFr = "fr",
            OneWayPrice = 100,
            RoundTripPrice = 100,
            SortOrder = 1,
            IsActive = true
        };

        var ctx = new ValidationContext(item);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(item, ctx, results, validateAllProperties: true);

        Assert.That(valid, Is.True);
        Assert.That(results, Is.Empty);
    }
}
