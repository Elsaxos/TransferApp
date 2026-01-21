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

        
        public async Task<IActionResult> Index()
        {
            var cars = await _db.Cars.ToListAsync();
            return View(cars);
        }

       
        public IActionResult Create()
        {
            return View(new Car());
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            
            var exists = await _db.Cars
                .AnyAsync(c => c.Registration == car.Registration);

            if (exists)
            {
                ModelState.AddModelError("Registration", "Вече има кола с такъв регистрационен номер.");
                return View(car);
            }

            
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



