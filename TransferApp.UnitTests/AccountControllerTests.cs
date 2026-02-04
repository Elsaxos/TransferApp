namespace TransferApp.UnitTests.Controllers
{
    [NUnit.Framework.TestFixture]
    public class AccountControllerTests
    {
        // ---- Helpers ----

        private static Microsoft.Extensions.Options.IOptions<TransferApp.Options.AdminUsersOptions> BuildAdminOptions(
            params (string Username, string PlainPassword)[] users)
        {
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<DummyUser>();
            var opts = new TransferApp.Options.AdminUsersOptions();

            foreach (var u in users)
            {
                var dummy = new DummyUser { Username = u.Username };
                var hash = hasher.HashPassword(dummy, u.PlainPassword);

                opts.Users.Add(new TransferApp.Options.AdminUser
                {
                    Username = u.Username,
                    PasswordHash = hash
                });
            }

            return Microsoft.Extensions.Options.Options.Create(opts);
        }

        private static TransferApp.Controllers.AccountController CreateControllerWithHttpContext(
      Microsoft.Extensions.Options.IOptions<TransferApp.Options.AdminUsersOptions> options,
      bool withFakeAuthService)
        {
            var controller = new TransferApp.Controllers.AccountController(options);

            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
            };

            
            controller.Url = new FakeUrlHelper();

            if (withFakeAuthService)
            {
                controller.HttpContext.RequestServices = new ServiceProviderStub();
            }

            return controller;
        }


        private class DummyUser
        {
            public string Username { get; set; } = "";
        }

        // Minimal stub to satisfy SignInAsync / SignOutAsync
        private class ServiceProviderStub : System.IServiceProvider
        {
            public object? GetService(System.Type serviceType)
            {
                if (serviceType == typeof(Microsoft.AspNetCore.Authentication.IAuthenticationService))
                    return new FakeAuthService();

                // UrlHelper / tempdata не ти трябва за тези тестове
                return null;
            }
        }

        private class FakeAuthService : Microsoft.AspNetCore.Authentication.IAuthenticationService
        {
            public System.Threading.Tasks.Task SignInAsync(
                Microsoft.AspNetCore.Http.HttpContext context,
                string? scheme,
                System.Security.Claims.ClaimsPrincipal principal,
                Microsoft.AspNetCore.Authentication.AuthenticationProperties? properties)
                => System.Threading.Tasks.Task.CompletedTask;

            public System.Threading.Tasks.Task SignOutAsync(
                Microsoft.AspNetCore.Http.HttpContext context,
                string? scheme,
                Microsoft.AspNetCore.Authentication.AuthenticationProperties? properties)
                => System.Threading.Tasks.Task.CompletedTask;

            public System.Threading.Tasks.Task<Microsoft.AspNetCore.Authentication.AuthenticateResult> AuthenticateAsync(
                Microsoft.AspNetCore.Http.HttpContext context,
                string? scheme)
                => System.Threading.Tasks.Task.FromResult(Microsoft.AspNetCore.Authentication.AuthenticateResult.NoResult());

            public System.Threading.Tasks.Task ChallengeAsync(
                Microsoft.AspNetCore.Http.HttpContext context,
                string? scheme,
                Microsoft.AspNetCore.Authentication.AuthenticationProperties? properties)
                => System.Threading.Tasks.Task.CompletedTask;

            public System.Threading.Tasks.Task ForbidAsync(
                Microsoft.AspNetCore.Http.HttpContext context,
                string? scheme,
                Microsoft.AspNetCore.Authentication.AuthenticationProperties? properties)
                => System.Threading.Tasks.Task.CompletedTask;
        }

        private class FakeUrlHelper : Microsoft.AspNetCore.Mvc.IUrlHelper
        {
            public Microsoft.AspNetCore.Mvc.ActionContext ActionContext { get; } =
                new Microsoft.AspNetCore.Mvc.ActionContext();

            public bool IsLocalUrl(string? url)
            {
                if (string.IsNullOrWhiteSpace(url)) return false;

                // local urls: "/x", "/x/y", "~/x"
                if (url.StartsWith("~/")) return true;

                if (url.StartsWith("/"))
                    return !url.StartsWith("//") && !url.StartsWith("/\\");

                return false;
            }

           
            public string? Action(Microsoft.AspNetCore.Mvc.Routing.UrlActionContext actionContext)
            {
                var action = actionContext.Action ?? "";
                var controller = actionContext.Controller ?? "";

                
                if (controller.Equals("Home", StringComparison.OrdinalIgnoreCase) &&
                    action.Equals("Index", StringComparison.OrdinalIgnoreCase))
                    return "/";

                return $"/{controller}/{action}";
            }

            public string? Content(string? contentPath) => contentPath;

            public string? Link(string? routeName, object? values) => "/";

            public string? RouteUrl(Microsoft.AspNetCore.Mvc.Routing.UrlRouteContext routeContext) => "/";
        }



        

        [NUnit.Framework.Test]
        public void Login_Get_Sets_ReturnUrl_And_Returns_View()
        {
            var opts = BuildAdminOptions(("admin1", "pass"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: false);

            var result = c.Login("/Admin/Inquiries");

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.ViewResult>());
            NUnit.Framework.Assert.That(c.ViewBag.ReturnUrl, NUnit.Framework.Is.EqualTo("/Admin/Inquiries"));
        }

        // ---- New tests ----

        [NUnit.Framework.Test]
        public async System.Threading.Tasks.Task Login_Post_Empty_Username_Returns_View_And_Bg_Error()
        {
            var opts = BuildAdminOptions(("admin1", "pass"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: false);

            var result = await c.Login("", "x", "/");

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.ViewResult>());
            NUnit.Framework.Assert.That((string?)c.ViewBag.Error, NUnit.Framework.Does.Contain("Моля въведи"));
        }

        [NUnit.Framework.Test]
        public async System.Threading.Tasks.Task Login_Post_Empty_Password_Returns_View_And_Bg_Error()
        {
            var opts = BuildAdminOptions(("admin1", "pass"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: false);

            var result = await c.Login("admin1", "", "/");

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.ViewResult>());
            NUnit.Framework.Assert.That((string?)c.ViewBag.Error, NUnit.Framework.Does.Contain("Моля въведи"));
        }

        [NUnit.Framework.Test]
        public async System.Threading.Tasks.Task Login_Post_Unknown_User_Returns_View_And_Generic_Error()
        {
            var opts = BuildAdminOptions(("admin1", "pass"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: false);

            var result = await c.Login("nope", "pass", "/");

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.ViewResult>());
            NUnit.Framework.Assert.That((string?)c.ViewBag.Error, NUnit.Framework.Does.Contain("Грешно"));
        }

        [NUnit.Framework.Test]
        public async System.Threading.Tasks.Task Login_Post_Wrong_Password_Returns_View_And_Generic_Error()
        {
            var opts = BuildAdminOptions(("admin1", "CorrectPassword"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: false);

            var result = await c.Login("admin1", "WrongPassword", "/");

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.ViewResult>());
            NUnit.Framework.Assert.That((string?)c.ViewBag.Error, NUnit.Framework.Does.Contain("Грешно"));
        }

        [NUnit.Framework.Test]
        public async System.Threading.Tasks.Task Login_Post_Valid_Admin_Local_ReturnUrl_Redirects_To_ReturnUrl()
        {
            var opts = BuildAdminOptions(("admin1", "TopSecret!1"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: true);

            // LocalUrl, allow
            var result = await c.Login("admin1", "TopSecret!1", "/Admin/Prices");

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.RedirectResult>());
            var redirect = (Microsoft.AspNetCore.Mvc.RedirectResult)result;
            NUnit.Framework.Assert.That(redirect.Url, NUnit.Framework.Is.EqualTo("/Admin/Prices"));
        }

        [NUnit.Framework.Test]
        public async System.Threading.Tasks.Task Login_Post_Valid_Admin_NonLocal_ReturnUrl_Ignores_And_Goes_To_Inquiries()
        {
            var opts = BuildAdminOptions(("admin1", "TopSecret!1"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: true);

            // Non-local should be ignored
            var result = await c.Login("admin1", "TopSecret!1", "https://evil.com");

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.RedirectResult>());
            var redirect = (Microsoft.AspNetCore.Mvc.RedirectResult)result;
            NUnit.Framework.Assert.That(redirect.Url, NUnit.Framework.Is.EqualTo("/Admin/Inquiries"));
        }

        [NUnit.Framework.Test]
        public void Denied_Returns_Content()
        {
            var opts = BuildAdminOptions(("admin1", "pass"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: false);

            var result = c.Denied();

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.ContentResult>());
            var content = (Microsoft.AspNetCore.Mvc.ContentResult)result;
            NUnit.Framework.Assert.That(content.Content, NUnit.Framework.Is.EqualTo("Access Denied"));
        }

        [NUnit.Framework.Test]
        public async System.Threading.Tasks.Task Logout_Post_Redirects_To_Home_Index()
        {
            var opts = BuildAdminOptions(("admin1", "pass"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: true);

            var result = await c.Logout(null);

            // RedirectToAction -> RedirectToActionResult
            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.RedirectToActionResult>());
            var redirect = (Microsoft.AspNetCore.Mvc.RedirectToActionResult)result;
            NUnit.Framework.Assert.That(redirect.ActionName, NUnit.Framework.Is.EqualTo("Index"));
            NUnit.Framework.Assert.That(redirect.ControllerName, NUnit.Framework.Is.EqualTo("Home"));
        }

        [NUnit.Framework.Test]
        public async System.Threading.Tasks.Task Logout_Post_With_Local_ReturnUrl_Redirects_To_It()
        {
            var opts = BuildAdminOptions(("admin1", "pass"));
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: true);

            var result = await c.Logout("/Pages/Contacts");

            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.RedirectResult>());
            var redirect = (Microsoft.AspNetCore.Mvc.RedirectResult)result;
            NUnit.Framework.Assert.That(redirect.Url, NUnit.Framework.Is.EqualTo("/Pages/Contacts"));
        }

        [Test]
        public async System.Threading.Tasks.Task Login_Post_Invalid_Hash_Shows_Config_Error_In_ViewBag()
        {
            // Arrange: invalid base64-ish string 
            var bad = new TransferApp.Options.AdminUsersOptions();
            bad.Users.Add(new TransferApp.Options.AdminUser
            {
                Username = "admin1",
                PasswordHash = "NOT_A_VALID_HASH"
            });

            var opts = Microsoft.Extensions.Options.Options.Create(bad);
            var c = CreateControllerWithHttpContext(opts, withFakeAuthService: false);

            // Act
            var result = await c.Login("admin1", "whatever", "/");

            // Assert
            NUnit.Framework.Assert.That(result, NUnit.Framework.Is.InstanceOf<Microsoft.AspNetCore.Mvc.ViewResult>());
            NUnit.Framework.Assert.That((string?)c.ViewBag.Error, NUnit.Framework.Does.Contain("Грешка в конфигурацията"));
        }
    }
}
