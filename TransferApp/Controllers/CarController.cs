using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferApp.Data;
using TransferApp.Models;

namespace TransferApp.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CarController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Car/Index
        public async Task<IActionResult> Index()
        {
            var cars = await _db.Cars.ToListAsync();
            return View(cars);
        }

        // GET: /Car/Create
        public IActionResult Create()
        {
            return View(new Car());
        }

        // POST: /Car/Create
        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            // Проверка за дублаж по регистрация (може да я оставиш или махнеш временно)
            var exists = await _db.Cars
                .AnyAsync(c => c.Registration == car.Registration);

            if (exists)
            {
                ModelState.AddModelError("Registration", "Вече има кола с такъв регистрационен номер.");
                return View(car);
            }

            // ТУК: ако ImageUrl липсва, записваме празен стринг вместо null
            if (string.IsNullOrWhiteSpace(car.ImageUrl))
                car.ImageUrl = "";

            if (!ModelState.IsValid)
                return View(car);

            _db.Cars.Add(car);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}



