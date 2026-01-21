using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferApp.Data;
using TransferApp.Models;
using TransferApp.ViewModels;

namespace TransferApp.Controllers
{
    public class TransferController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TransferController(ApplicationDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new TransferCreateViewModel
            {
                PickupDateTime = DateTime.Now.AddHours(1),
                Passengers = 1,
                TripType = "OneWay",
                ActivePrices = await _db.PriceItems
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.SortOrder).ThenBy(p => p.Id)
                    .ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransferCreateViewModel vm, string submitType)
        {
            
            vm.ActivePrices = await _db.PriceItems
                .Where(p => p.IsActive)
                .OrderBy(p => p.SortOrder).ThenBy(p => p.Id)
                .ToListAsync();

            // базови trim
            vm.CustomerName = (vm.CustomerName ?? "").Trim();
            vm.Phone = (vm.Phone ?? "").Trim();
            vm.Email = (vm.Email ?? "").Trim();
            vm.PickupAddress = (vm.PickupAddress ?? "").Trim();
            vm.DropoffAddress = (vm.DropoffAddress ?? "").Trim();
            vm.Notes = (vm.Notes ?? "").Trim();

            if (!ModelState.IsValid)
                return View(vm);

            var priceItem = await _db.PriceItems.FirstOrDefaultAsync(p => p.Id == vm.PriceItemId && p.IsActive);
            if (priceItem == null)
            {
                ModelState.AddModelError(nameof(vm.PriceItemId), "Моля изберете валиден маршрут.");
                return View(vm);
            }

            var price = vm.TripType == "RoundTrip" ? priceItem.RoundTripPrice : priceItem.OneWayPrice;

            var status = submitType == "Reserve" ? "Резервация" : "Запитване";

            var req = new TransferRequest
            {
                CustomerName = vm.CustomerName,
                Phone = vm.Phone,
                Email = vm.Email,
                PickupAddress = vm.PickupAddress,
                DropoffAddress = vm.DropoffAddress,
                PickupDateTime = vm.PickupDateTime,
                Passengers = vm.Passengers,
                Notes = vm.Notes,
                Price = price,
                Status = status
            };

            _db.TransferRequests.Add(req);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Thanks));
        }

        [HttpGet]
        public IActionResult Thanks() => View();
    }
}



