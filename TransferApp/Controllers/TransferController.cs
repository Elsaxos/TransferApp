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
        public IActionResult Create()
        {
            return View(new TransferRequest
            {
                PickupDateTime = DateTime.Now.AddHours(1),
                Passengers = 1
            });
        }

        // POST: /Transfer/Create
        [HttpPost]
        public async Task<IActionResult> Create(TransferRequest req)
        {
            // ако Notes не е попълнено (null или празно) -> записваме празен текст
            if (string.IsNullOrWhiteSpace(req.Notes))
                req.Notes = "";

            // можеш да зададеш и статус, за всеки случай
            if (string.IsNullOrWhiteSpace(req.Status))
                req.Status = "Нова";

            // временно фиксирана цена
            req.Price = 50;

            _db.TransferRequests.Add(req);
            await _db.SaveChangesAsync();

            return RedirectToAction("Thanks");
        }


        public IActionResult Thanks()
        {
            return View();
        }
    }
}

