using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferApp.Data;

namespace TransferApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    [Route("Admin")]
    public class AdminTransfersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminTransfersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Admin/Inquiries
        [HttpGet("Inquiries")]
        public async Task<IActionResult> Inquiries()
        {
            var items = await _db.TransferRequests
                .Where(x => x.Status == "Запитване")
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View("~/Views/AdminTransfers/Inquiries.cshtml", items);
        }

        // GET: /Admin/Reservations
        [HttpGet("Reservations")]
        public async Task<IActionResult> Reservations()
        {
            var items = await _db.TransferRequests
                .Where(x => x.Status == "Резервация")
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View("~/Views/AdminTransfers/Reservations.cshtml", items);
        }

        // GET: /Admin/Requests/{id}
        [HttpGet("Requests/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var req = await _db.TransferRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (req == null) return NotFound();

            return View("~/Views/AdminTransfers/Details.cshtml", req);
        }

        // GET: /Admin/Requests/Delete/{id}
        [HttpGet("Requests/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var req = await _db.TransferRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (req == null) return NotFound();

            return View("~/Views/AdminTransfers/Delete.cshtml", req);
        }

        // POST: /Admin/Requests/Delete/{id}
        [HttpPost("Requests/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var req = await _db.TransferRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (req == null) return NotFound();

            var status = req.Status;

            _db.TransferRequests.Remove(req);
            await _db.SaveChangesAsync();

            if (status == "Резервация")
                return RedirectToAction(nameof(Reservations));

            return RedirectToAction(nameof(Inquiries));
        }
    }
}
