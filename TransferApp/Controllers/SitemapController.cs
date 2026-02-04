using Microsoft.AspNetCore.Mvc;

namespace TransferApp.Controllers
{
    public class SitemapController : Controller
    {
        [HttpGet("/sitemap.xml")]
        public IActionResult Sitemap()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var urls = new[]
            {
                $"{baseUrl}/",
                $"{baseUrl}/Pages/About",
                $"{baseUrl}/Pages/Prices",
                $"{baseUrl}/Pages/Contacts",
                $"{baseUrl}/Home/Privacy",
                $"{baseUrl}/Pages/Cookies"
            };

            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + "\n" +
                      @"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">" + "\n" +
                      string.Join("\n", urls.Select(u =>
                        $"  <url><loc>{u}</loc><changefreq>weekly</changefreq><priority>0.8</priority></url>")) + "\n" +
                      @"</urlset>";

            return Content(xml, "application/xml");
        }
    }
}

