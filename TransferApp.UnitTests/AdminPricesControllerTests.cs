using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TransferApp.Controllers;
using TransferApp.Models;
using TransferApp.UnitTests.TestHelpers;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class AdminPricesControllerTests
{
    [Test]
    public async Task Index_Returns_View_With_List()
    {
        using var db = TestDb.CreateContext(nameof(Index_Returns_View_With_List));
        db.PriceItems.Add(new PriceItem { RouteKey = "k", RouteBg = "bg", RouteEn = "en", RouteRu = "ru", RouteFr = "fr", OneWayPrice = 1, RoundTripPrice = 2, SortOrder = 1, IsActive = true });
        db.SaveChanges();

        var c = new AdminPricesController(db);
        var result = await c.Index();

        Assert.That(result, Is.InstanceOf<ViewResult>());
        var vr = (ViewResult)result;
        Assert.That(vr.Model, Is.Not.Null);
    }

    [Test]
    public async Task Create_Post_Invalid_Model_Returns_View_With_Model()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_Invalid_Model_Returns_View_With_Model));
        var c = new AdminPricesController(db);
        c.ModelState.AddModelError("x", "err");

        var model = new PriceItem();
        var result = await c.Create(model);

        Assert.That(result, Is.InstanceOf<ViewResult>());
    }

    [Test]
    public async Task Create_Post_Valid_Model_Sets_IsActive_And_Redirects()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_Valid_Model_Sets_IsActive_And_Redirects));
        var c = new AdminPricesController(db);

        var model = new PriceItem
        {
            RouteKey = "sofia-varna",
            RouteBg = "София → Варна",
            RouteEn = "Sofia → Varna",
            RouteRu = "София → Варна",
            RouteFr = "Sofia → Varna",
            OneWayPrice = 10,
            RoundTripPrice = 18,
            SortOrder = 1
        };

        var result = await c.Create(model);

        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        Assert.That(db.PriceItems.Count(), Is.EqualTo(1));
        Assert.That(db.PriceItems.First().IsActive, Is.True);
    }

    [Test]
    public async Task Edit_Get_NotFound_When_Missing()
    {
        using var db = TestDb.CreateContext(nameof(Edit_Get_NotFound_When_Missing));
        var c = new AdminPricesController(db);

        var result = await c.Edit(123);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task Edit_Post_BadRequest_When_Id_Mismatch()
    {
        using var db = TestDb.CreateContext(nameof(Edit_Post_BadRequest_When_Id_Mismatch));
        var c = new AdminPricesController(db);

        var model = new PriceItem { Id = 5 };
        var result = await c.Edit(6, model);

        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }
}
