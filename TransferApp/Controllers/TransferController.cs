using Microsoft.AspNetCore.Mvc;
using TransferApp.Data;
using TransferApp.Models;

namespace TransferApp.Controllers
{
    public class TransferController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TransferController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Transfer/Create
        [HttpGet]
        public IActionResult Create(string? customerName = null, string? phone = null, string? notes = null)
        {
            // Ако идваш от контактната форма (query string), префилваме
            return View(new TransferRequest
            {
                CustomerName = customerName ?? "",
                Phone = phone ?? "",
                Notes = notes ?? "",
                PickupDateTime = DateTime.Now.AddHours(1),
                Passengers = 1,
                Status = "Запитване" // или "Нова" - избери 1 стандарт
            });
        }

        // POST: /Transfer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransferRequest req)
        {
            // Нормализация (да не влизат null-и)
            req.CustomerName = (req.CustomerName ?? "").Trim();
            req.Phone = (req.Phone ?? "").Trim();
            req.PickupAddress = (req.PickupAddress ?? "").Trim();
            req.DropoffAddress = (req.DropoffAddress ?? "").Trim();
            req.Notes = (req.Notes ?? "").Trim();

            if (string.IsNullOrWhiteSpace(req.Status))
                req.Status = "Запитване"; // или "Нова" - пак 1 стандарт

            // Минимална валидация, за да не се чупи/да не записва боклук
            if (string.IsNullOrWhiteSpace(req.CustomerName))
                ModelState.AddModelError(nameof(req.CustomerName), "Моля въведи име.");

            if (string.IsNullOrWhiteSpace(req.Phone))
                ModelState.AddModelError(nameof(req.Phone), "Моля въведи телефон.");

            if (string.IsNullOrWhiteSpace(req.PickupAddress))
                ModelState.AddModelError(nameof(req.PickupAddress), "Моля въведи адрес/място на тръгване.");

            if (string.IsNullOrWhiteSpace(req.DropoffAddress))
                ModelState.AddModelError(nameof(req.DropoffAddress), "Моля въведи адрес/място на пристигане.");

            if (req.Passengers < 1)
                ModelState.AddModelError(nameof(req.Passengers), "Пътниците трябва да са поне 1.");

            if (!ModelState.IsValid)
                return View(req);

            // временно фиксирана цена
            req.Price = 50;

            _db.TransferRequests.Add(req);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Thanks));
        }

        // POST: /Transfer/QuickRequest  (ако все още го ползваш)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickRequest(string customerName, string phone, string notes)
        {
            customerName = (customerName ?? "").Trim();
            phone = (phone ?? "").Trim();
            notes = (notes ?? "").Trim();

            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(phone))
            {
                // Връщаме те към Contacts или където ти е формата
                TempData["Error"] = "Моля попълни име и телефон.";
                return Redirect("/Pages/Contacts");
            }

            var request = new TransferRequest
            {
                CustomerName = customerName,
                Phone = phone,
                Notes = notes,
                PickupAddress = "Контактна форма",
                DropoffAddress = "Контактна форма",
                PickupDateTime = DateTime.Now.AddMinutes(5),
                Passengers = 1,
                Price = 0,
                Status = "Запитване"
            };

            _db.TransferRequests.Add(request);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Thanks));
        }

        [HttpGet]
        public IActionResult Thanks()
        {
            return View();
        }
    }
}


