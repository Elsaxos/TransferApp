using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferApp.Data;

namespace TransferApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Admin/Index
        public async Task<IActionResult> Index()
        {
            var requests = await _db.TransferRequests
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            return View(requests);
        }
    }
}

