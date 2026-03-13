using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TransferApp.Controllers;
using TransferApp.Models;
using TransferApp.UnitTests.TestHelpers;
using TransferApp.ViewModels;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class TransferControllerTests
{
    [Test]
    public async Task Create_Get_Populates_Defaults_And_Active_Prices()
    {
        using var db = TestDb.CreateContext(nameof(Create_Get_Populates_Defaults_And_Active_Prices));
        db.PriceItems.Add(new PriceItem
        {
            RouteKey = "A",
            RouteBg = "A",
            RouteEn = "A",
            RouteRu = "A",
            RouteFr = "A",
            OneWayPrice = 10,
            RoundTripPrice = 18,
            SortOrder = 1,
            IsActive = true
        });
        db.PriceItems.Add(new PriceItem
        {
            RouteKey = "B",
            RouteBg = "B",
            RouteEn = "B",
            RouteRu = "B",
            RouteFr = "B",
            OneWayPrice = 20,
            RoundTripPrice = 38,
            SortOrder = 2,
            IsActive = false
        });
        db.SaveChanges();

        var c = new TransferController(db);
        var result = await c.Create();

        Assert.That(result, Is.InstanceOf<ViewResult>());
        var vr = (ViewResult)result;
        Assert.That(vr.Model, Is.InstanceOf<TransferCreateViewModel>());

        var vm = (TransferCreateViewModel)vr.Model!;
        Assert.That(vm.Passengers, Is.EqualTo(1));
        Assert.That(vm.TripType, Is.EqualTo("OneWay"));
        Assert.That(vm.ActivePrices.Count, Is.EqualTo(1));

        var now = DateTime.Now;
        Assert.That(vm.PickupDateTime, Is.GreaterThan(now.AddMinutes(50)));
        Assert.That(vm.PickupDateTime, Is.LessThan(now.AddMinutes(70)));
    }

    [Test]
    public async Task Create_Post_Invalid_Model_Returns_View_And_Repopulates_Prices()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_Invalid_Model_Returns_View_And_Repopulates_Prices));
        db.PriceItems.Add(new PriceItem
        {
            RouteKey = "A",
            RouteBg = "A",
            RouteEn = "A",
            RouteRu = "A",
            RouteFr = "A",
            OneWayPrice = 10,
            RoundTripPrice = 18,
            SortOrder = 1,
            IsActive = true
        });
        db.SaveChanges();

        var c = new TransferController(db);
        c.ModelState.AddModelError("x", "err");

        var vm = new TransferCreateViewModel();
        var result = await c.Create(vm, "Reserve");

        Assert.That(result, Is.InstanceOf<ViewResult>());
        var vr = (ViewResult)result;
        var model = (TransferCreateViewModel)vr.Model!;
        Assert.That(model.ActivePrices.Count, Is.EqualTo(1));
        Assert.That(db.TransferRequests.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task Create_Post_Invalid_PriceItem_Returns_View_With_ModelError()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_Invalid_PriceItem_Returns_View_With_ModelError));
        db.PriceItems.Add(new PriceItem
        {
            RouteKey = "A",
            RouteBg = "A",
            RouteEn = "A",
            RouteRu = "A",
            RouteFr = "A",
            OneWayPrice = 10,
            RoundTripPrice = 18,
            SortOrder = 1,
            IsActive = true
        });
        db.SaveChanges();

        var c = new TransferController(db);
        var vm = new TransferCreateViewModel
        {
            CustomerName = "Ivan",
            Phone = "123",
            Email = "ivan@example.com",
            PickupAddress = "A",
            DropoffAddress = "B",
            PickupDateTime = DateTime.Now.AddHours(2),
            Passengers = 2,
            PriceItemId = 999,
            TripType = "OneWay"
        };

        var result = await c.Create(vm, "Reserve");

        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(c.ModelState.ContainsKey(nameof(TransferCreateViewModel.PriceItemId)), Is.True);
        Assert.That(db.TransferRequests.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task Create_Post_OneWay_Inquiry_Saves_Request_With_OneWay_Price()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_OneWay_Inquiry_Saves_Request_With_OneWay_Price));
        var price = new PriceItem
        {
            RouteKey = "A",
            RouteBg = "A",
            RouteEn = "A",
            RouteRu = "A",
            RouteFr = "A",
            OneWayPrice = 10,
            RoundTripPrice = 18,
            SortOrder = 1,
            IsActive = true
        };
        db.PriceItems.Add(price);
        db.SaveChanges();

        var c = new TransferController(db);
        var vm = new TransferCreateViewModel
        {
            CustomerName = " Ivan ",
            Phone = " 123 ",
            Email = "ivan@example.com",
            PickupAddress = " A ",
            DropoffAddress = " B ",
            PickupDateTime = DateTime.Now.AddHours(2),
            Passengers = 2,
            PriceItemId = price.Id,
            TripType = "OneWay",
            Notes = " note "
        };

        var result = await c.Create(vm, "Inquiry");

        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        Assert.That(db.TransferRequests.Count(), Is.EqualTo(1));

        var saved = db.TransferRequests.First();
        Assert.That(saved.Price, Is.EqualTo(10));
        Assert.That(saved.Status, Is.EqualTo("Запитване"));
        Assert.That(saved.CustomerName, Is.EqualTo("Ivan"));
        Assert.That(saved.Phone, Is.EqualTo("123"));
        Assert.That(saved.PickupAddress, Is.EqualTo("A"));
        Assert.That(saved.DropoffAddress, Is.EqualTo("B"));
        Assert.That(saved.Notes, Is.EqualTo("note"));
    }

    [Test]
    public async Task Create_Post_RoundTrip_Reserve_Saves_Request_With_RoundTrip_Price()
    {
        using var db = TestDb.CreateContext(nameof(Create_Post_RoundTrip_Reserve_Saves_Request_With_RoundTrip_Price));
        var price = new PriceItem
        {
            RouteKey = "A",
            RouteBg = "A",
            RouteEn = "A",
            RouteRu = "A",
            RouteFr = "A",
            OneWayPrice = 10,
            RoundTripPrice = 18,
            SortOrder = 1,
            IsActive = true
        };
        db.PriceItems.Add(price);
        db.SaveChanges();

        var c = new TransferController(db);
        var vm = new TransferCreateViewModel
        {
            CustomerName = "Ivan",
            Phone = "123",
            Email = "ivan@example.com",
            PickupAddress = "A",
            DropoffAddress = "B",
            PickupDateTime = DateTime.Now.AddHours(2),
            Passengers = 2,
            PriceItemId = price.Id,
            TripType = "RoundTrip"
        };

        var result = await c.Create(vm, "Reserve");

        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var saved = db.TransferRequests.First();
        Assert.That(saved.Price, Is.EqualTo(18));
        Assert.That(saved.Status, Is.EqualTo("Резервация"));
    }
}
