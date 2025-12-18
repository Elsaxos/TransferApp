using Microsoft.AspNetCore.Mvc;

namespace TransferApp.Controllers
{
    public class PagesController : Controller
    {
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Prices()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contacts()
        {
            return View();
        }
    }
}


