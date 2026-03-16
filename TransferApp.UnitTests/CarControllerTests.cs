using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TransferApp.Controllers;
using TransferApp.Models;
using TransferApp.UnitTests.TestHelpers;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class CarControllerTests
{
    [Test]
    public async Task Index_Returns_View_With_Cars()
    {
        using var db = TestDb.CreateContext(nameof(Index_Returns_View_With_Cars));
        db.Cars.Add(new Car { Make = "VW", Model = "Golf", Registration = "CA1234AA", Seats = 4 });
        db.SaveChanges();

        var c = new CarController(db);
        var result = await c.Index();

        Assert.That(result, Is.InstanceOf<ViewResult>());
        var vr = (ViewResult)result;
        Assert.That(vr.Model, Is.Not.Null);
    }

    [Test]
    public void Create_Get_Returns_New_Car()
    {
        using var db = TestDb.CreateContext(nameof(Create_Get_Returns_New_Car));
        var c = new CarController(db);

        var result = c.Create();

        Assert.That(result, Is.InstanceOf<ViewResult>());
        var vr = (ViewResult)result;
        Assert.That(vr.Model, Is.InstanceOf<Car>());
    }

    [Test]
    public async Task Create_Post_Duplicate_Registration_Returns_View_With_Error()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_Duplicate_Registration_Returns_View_With_Error));
        db.Cars.Add(new Car { Make = "VW", Model = "Golf", Registration = "CA1234AA", Seats = 4 });
        db.SaveChanges();

        var c = new CarController(db);
        var car = new Car { Make = "Audi", Model = "A4", Registration = "CA1234AA", Seats = 4 };

        var result = await c.Create(car);

        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(c.ModelState.ContainsKey("Registration"), Is.True);
    }

    [Test]
    public async Task Create_Post_Blank_ImageUrl_Sets_Empty_And_Saves()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_Blank_ImageUrl_Sets_Empty_And_Saves));
        var c = new CarController(db);
        var car = new Car { Make = "VW", Model = "Golf", Registration = "CA9999BB", Seats = 4, ImageUrl = "   " };

        var result = await c.Create(car);

        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var saved = db.Cars.First();
        Assert.That(saved.ImageUrl, Is.EqualTo(""));
    }

    [Test]
    public async Task Create_Post_Invalid_Model_Returns_View_And_Does_Not_Save()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_Invalid_Model_Returns_View_And_Does_Not_Save));
        var c = new CarController(db);
        c.ModelState.AddModelError("Make", "err");

        var car = new Car { Make = "VW", Model = "Golf", Registration = "CA7777CC", Seats = 4 };
        var result = await c.Create(car);

        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(db.Cars.Count(), Is.EqualTo(0));
    }
}
