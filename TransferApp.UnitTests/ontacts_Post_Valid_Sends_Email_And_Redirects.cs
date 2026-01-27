using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using TransferApp.ViewModels;

namespace TransferApp.UnitTests;

[TestFixture]
public class ContactFormViewModelTests
{
    [Test]
    public void Name_And_Phone_Are_Required()
    {
        var vm = new ContactFormViewModel
        {
            Name = "",
            Phone = "",
            Message = "Hello"
        };

        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        Assert.That(isValid, Is.False);

        Assert.That(results, Has.Some.Matches<ValidationResult>(
            r => r.MemberNames.Contains(nameof(ContactFormViewModel.Name))));

        Assert.That(results, Has.Some.Matches<ValidationResult>(
            r => r.MemberNames.Contains(nameof(ContactFormViewModel.Phone))));
    }
}


