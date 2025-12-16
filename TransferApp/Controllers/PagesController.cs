using Microsoft.AspNetCore.Mvc;

namespace TransferApp.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Prices()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }
    }
}
