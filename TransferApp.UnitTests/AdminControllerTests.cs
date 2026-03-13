using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TransferApp.Controllers;
using TransferApp.Models;
using TransferApp.UnitTests.TestHelpers;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class AdminControllerTests
{
    [Test]
    public async Task Index_Returns_ViewModel_With_Inquiries_And_Reservations()
    {
        using var db = TestDb.CreateContext(nameof(Index_Returns_ViewModel_With_Inquiries_And_Reservations));
        db.TransferRequests.Add(new TransferRequest
        {
            CustomerName = "A",
            Phone = "1",
            Email = "a@a.com",
            PickupAddress = "x",
            DropoffAddress = "y",
            PickupDateTime = DateTime.UtcNow,
            Passengers = 1,
            Price = 10,
            Status = "Запитване"
        });
        db.TransferRequests.Add(new TransferRequest
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
        });
        db.SaveChanges();

        var c = new AdminController(db);
        var result = await c.Index();

        Assert.That(result, Is.InstanceOf<ViewResult>());
        var vr = (ViewResult)result;
        Assert.That(vr.Model, Is.Not.Null);
        var modelType = vr.Model!.GetType();
        var inquiriesProp = modelType.GetProperty("Inquiries");
        var reservationsProp = modelType.GetProperty("Reservations");

        Assert.That(inquiriesProp, Is.Not.Null);
        Assert.That(reservationsProp, Is.Not.Null);

        var inquiries = (System.Collections.ICollection?)inquiriesProp!.GetValue(vr.Model);
        var reservations = (System.Collections.ICollection?)reservationsProp!.GetValue(vr.Model);

        Assert.That(inquiries?.Count, Is.EqualTo(1));
        Assert.That(reservations?.Count, Is.EqualTo(1));
    }

    [Test]
    public void Prices_Redirects_To_AdminPrices_Index()
    {
        using var db = TestDb.CreateContext(nameof(Prices_Redirects_To_AdminPrices_Index));
        var c = new AdminController(db);

        var result = c.Prices();

        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var redirect = (RedirectToActionResult)result;
        Assert.That(redirect.ControllerName, Is.EqualTo("AdminPrices"));
        Assert.That(redirect.ActionName, Is.EqualTo("Index"));
    }
}
