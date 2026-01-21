using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferApp.Data;
using TransferApp.Models;

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
            var prices = await _db.PriceItems
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Id)
                .ToListAsync();

            return View(prices); 
        }

        
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/PricesCreate.cshtml");
        }

        
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PriceItem model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/PricesCreate.cshtml", model);
            }

            model.IsActive = true;

            _db.PriceItems.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.PriceItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/PricesEdit.cshtml", item);
        }

        
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PriceItem model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/PricesEdit.cshtml", model);
            }

            _db.PriceItems.Update(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}



