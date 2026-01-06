using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferApp.Data;
using TransferApp.Models;

namespace TransferApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // нормализираме Status: null -> "", trim
            var inquiries = await _db.TransferRequests
                .Where(r => ((r.Status ?? "").Trim()) == "Запитване")
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            var reservations = await _db.TransferRequests
                .Where(r => ((r.Status ?? "").Trim()) != "Запитване")
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                Inquiries = inquiries,
                Reservations = reservations
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Prices()
        {
            var items = await _db.PriceItems
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Id)
                .ToListAsync();

            return View(items); // <-- ще търси Views/Admin/Prices.cshtml
        }

    }
}




