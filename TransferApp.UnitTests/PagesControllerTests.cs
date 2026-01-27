using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TransferApp.Controllers;
using TransferApp.UnitTests.TestHelpers;
using TransferApp.ViewModels;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class PagesControllerTests
{
    [Test]
    public async Task Contacts_Post_Invalid_Model_Returns_View()
    {
        using var db = TestDb.CreateContext(nameof(Contacts_Post_Invalid_Model_Returns_View));
        var email = new FakeEmailSender();
        var c = new PagesController(db, email);

        // важно: TempData да не е null (за всеки случай)
        AttachTempData(c);

        c.ModelState.AddModelError("x", "err");

        var vm = new ContactFormViewModel();
        var result = await c.Contacts(vm);

        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(email.SentCount, Is.EqualTo(0));
    }

    [Test]
    public async Task Contacts_Post_Valid_Sends_Email_And_Redirects()
    {
        using var db = TestDb.CreateContext(nameof(Contacts_Post_Valid_Sends_Email_And_Redirects));
        var email = new FakeEmailSender();
        var c = new PagesController(db, email);

        AttachTempData(c);

        var vm = new ContactFormViewModel
        {
            Name = "Konstantin",
            Phone = "+359...",
            Message = "Hello"
        };

        var result = await c.Contacts(vm);

        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());

       
        Assert.That(email.SentCount, Is.EqualTo(1));
        Assert.That(email.LastSubject, Does.Contain("ProTransfer"));
    }

    private static void AttachTempData(Controller c)
    {
        var http = new DefaultHttpContext();
        c.ControllerContext = new ControllerContext { HttpContext = http };
        c.TempData = new TempDataDictionary(http, new DummyTempDataProvider());
    }

    private sealed class DummyTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object?> LoadTempData(HttpContext context) => new Dictionary<string, object?>();
        public void SaveTempData(HttpContext context, IDictionary<string, object?> values) { }
    }
}


