using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using TransferApp.ViewModels;

namespace TransferApp.UnitTests;

[TestFixture]
public class ViewModelValidationTests
{
    [Test]
    public void TransferCreateViewModel_Required_Fields_Are_Enforced()
    {
        var vm = new TransferCreateViewModel
        {
            CustomerName = "",
            Phone = "",
            Email = "bad",
            PickupAddress = "",
            DropoffAddress = "",
            Passengers = 0,
            PriceItemId = null,
            TripType = ""
        };

        var ctx = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(vm, ctx, results, validateAllProperties: true);

        Assert.That(valid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferCreateViewModel.CustomerName))), Is.True);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferCreateViewModel.Phone))), Is.True);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferCreateViewModel.Email))), Is.True);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferCreateViewModel.PickupAddress))), Is.True);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferCreateViewModel.DropoffAddress))), Is.True);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferCreateViewModel.Passengers))), Is.True);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferCreateViewModel.PriceItemId))), Is.True);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(TransferCreateViewModel.TripType))), Is.True);
    }

    [Test]
    public void ContactFormViewModel_Email_Is_Required_And_Valid()
    {
        var vm = new ContactFormViewModel
        {
            Name = "Ivan",
            Phone = "123",
            Email = "bad"
        };

        var ctx = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(vm, ctx, results, validateAllProperties: true);
        Assert.That(valid, Is.False);
        Assert.That(results.Any(r => r.MemberNames.Contains(nameof(ContactFormViewModel.Email))), Is.True);

        vm.Email = "ivan@example.com";
        results.Clear();

        valid = Validator.TryValidateObject(vm, new ValidationContext(vm), results, true);
        Assert.That(valid, Is.True);
    }
}
