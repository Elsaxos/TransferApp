using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using TransferApp.Models;

namespace TransferApp.UnitTests;

public class TransferRequestModelTests
{
    [Test]
    public void TransferRequest_Defaults_Should_Be_Correct()
    {
        var tr = new TransferRequest();

        Assert.That(tr.CustomerName, Is.EqualTo(string.Empty));
        Assert.That(tr.Phone, Is.EqualTo(string.Empty));
        Assert.That(tr.Email, Is.EqualTo(string.Empty));
        Assert.That(tr.PickupAddress, Is.EqualTo(string.Empty));
        Assert.That(tr.DropoffAddress, Is.EqualTo(string.Empty));
        Assert.That(tr.Status, Is.EqualTo("Запитване"));
        Assert.That(tr.Notes, Is.EqualTo(""));
    }

    [Test]
    public void TransferRequest_Email_Is_Required_And_Must_Be_Valid()
    {
        var tr = new TransferRequest
        {
            CustomerName = "Ivan",
            Phone = "123",
            Email = "not-an-email",
            PickupAddress = "A",
            DropoffAddress = "B",
            PickupDateTime = DateTime.UtcNow.AddDays(1),
            Passengers = 2,
            Price = 10m
        };

        var ctx = new ValidationContext(tr);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(tr, ctx, results, validateAllProperties: true);
        Assert.That(valid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferRequest.Email))), Is.True);

        // сега валиден email
        tr.Email = "ivan@example.com";
        results.Clear();

        valid = Validator.TryValidateObject(tr, new ValidationContext(tr), results, true);
        Assert.That(valid, Is.True);
        Assert.That(results, Is.Empty);
    }
}
