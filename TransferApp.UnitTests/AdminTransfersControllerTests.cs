using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TransferApp.Controllers;
using TransferApp.Models;
using TransferApp.UnitTests.TestHelpers;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class AdminTransfersControllerTests
{
    [Test]
    public async Task Details_Returns_NotFound_When_Missing()
    {
        using var db = TestDb.CreateContext(nameof(Details_Returns_NotFound_When_Missing));
        var c = new AdminTransfersController(db);

        var result = await c.Details(1);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task Inquiries_Returns_View()
    {
        using var db = TestDb.CreateContext(nameof(Inquiries_Returns_View));
        db.TransferRequests.Add(new TransferRequest { CustomerName = "A", Phone = "1", Email = "a@a.com", PickupAddress = "x", DropoffAddress = "y", PickupDateTime = DateTime.UtcNow, Passengers = 1, Price = 10, Status = "Запитване" });
        db.SaveChanges();

        var c = new AdminTransfersController(db);
        var result = await c.Inquiries();

        Assert.That(result, Is.InstanceOf<ViewResult>());
    }

    [Test]
    public async Task Reservations_Returns_View()
    {
        using var db = TestDb.CreateContext(nameof(Reservations_Returns_View));
        db.TransferRequests.Add(new TransferRequest { CustomerName = "B", Phone = "2", Email = "b@b.com", PickupAddress = "x", DropoffAddress = "y", PickupDateTime = DateTime.UtcNow, Passengers = 2, Price = 20, Status = "Резервация" });
        db.SaveChanges();

        var c = new AdminTransfersController(db);
        var result = await c.Reservations();

        Assert.That(result, Is.InstanceOf<ViewResult>());
    }

    [Test]
    public async Task DeleteConfirmed_NotFound_When_Missing()
    {
        using var db = TestDb.CreateContext(nameof(DeleteConfirmed_NotFound_When_Missing));
        var c = new AdminTransfersController(db);

        var result = await c.DeleteConfirmed(999);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteConfirmed_Redirects_Based_On_Status()
    {
        using var db = TestDb.CreateContext(nameof(DeleteConfirmed_Redirects_Based_On_Status));
        var req = new TransferRequest
        {
            CustomerName = "B",
            Phone = "2",
            Email = "b@b.com",
            PickupAddress = "x",
            DropoffAddress = "y",
            PickupDateTime = DateTime.UtcNow,
            Passengers = 2,
            Price = 20,
            Status = "Резервация"
        };
        db.TransferRequests.Add(req);
        db.SaveChanges();

        var c = new AdminTransfersController(db);
        var result = await c.DeleteConfirmed(req.Id);

        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var r = (RedirectToActionResult)result;
        Assert.That(r.ActionName, Is.EqualTo("Reservations"));
    }
}
