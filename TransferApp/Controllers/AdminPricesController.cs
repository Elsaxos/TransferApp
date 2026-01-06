using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferApp.Data;

namespace TransferApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/Prices")]
    public class AdminPricesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminPricesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var items = await _db.PriceItems
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Id)
                .ToListAsync();

            return View(items);
        }
    }
}



