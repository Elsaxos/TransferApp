using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferApp.Data;
using TransferApp.Services;
using TransferApp.ViewModels;

namespace TransferApp.Controllers
{
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _email;

        public PagesController(ApplicationDbContext db, IEmailSender email)
        {
            _db = db;
            _email = email;
        }

        [HttpGet]
        public async Task<IActionResult> Prices()
        {
            var prices = await _db.PriceItems
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Id)
                .ToListAsync();

            var vm = new PricesPublicViewModel
            {
                Prices = prices
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Contacts()
        {
            return View(new ContactFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contacts(ContactFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var subject = "Contact form – ProTransfer";

            var body = $@"
                <h3>Нов контакт</h3>
                <p><strong>Име:</strong> {vm.Name}</p>
                <p><strong>Телефон:</strong> {vm.Phone}</p>
                <p><strong>Съобщение:</strong><br/>{vm.Message}</p>
            ";

            await _email.SendEmailAsync(
                "Konstantin_stfnv@yahoo.com",
                subject,
                body
            );

            TempData["Success"] = "OK";
            return RedirectToAction(nameof(Contacts));
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Cookies()
        {
            return View();
        }

    }
}




