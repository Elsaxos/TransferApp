using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TransferApp.Options;

namespace TransferApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AdminUsersOptions _adminUsers;
        private readonly PasswordHasher<AdminUser> _hasher = new();

        public AccountController(IOptions<AdminUsersOptions> adminUsers)
        {
            _adminUsers = adminUsers.Value ?? new AdminUsersOptions();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Моля въведи потребителско име и парола.";
                return View();
            }

            var admin = _adminUsers.Users
                .FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

            if (admin == null)
            {
                ViewBag.Error = "Грешно потребителско име или парола.";
                return View();
            }

            var hash = (admin.PasswordHash ?? "").Trim();

            try
            {
                var result = _hasher.VerifyHashedPassword(admin, hash, password);

                if (result == PasswordVerificationResult.Failed)
                {
                    ViewBag.Error = "Грешно потребителско име или парола.";
                    return View();
                }
            }
            catch (FormatException)
            {
                ViewBag.Error = "Грешка в конфигурацията на admin hash-а (appsettings.json). Пейстни hash-а отново без интервали/нови редове.";
                return View();
            }

            // Успешен login -> cookie + role Admin
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    AllowRefresh = true
                });

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("/Admin/Inquiries");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Denied()
        {
            return Content("Access Denied");
        }
    }
}
