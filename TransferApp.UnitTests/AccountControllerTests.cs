using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TransferApp.Controllers;

namespace TransferApp.UnitTests.Controllers;

[TestFixture]
public class AccountControllerTests
{
    [Test]
    public void Login_Get_Sets_ReturnUrl_And_Returns_View()
    {
        var c = new AccountController();
        var result = c.Login("/Admin/Inquiries");

        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(c.ViewBag.ReturnUrl, Is.EqualTo("/Admin/Inquiries"));
    }

    [Test]
    public async Task Login_Post_With_Valid_Admin_Redirects_To_Admin_Inquiries()
    {
        var controller = new AccountController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // fake auth service so SignInAsync doesn't throw
        controller.HttpContext.RequestServices = new ServiceProviderStub();

        var result = await controller.Login("admin", "admin123", null);

        Assert.That(result, Is.InstanceOf<RedirectResult>());
        var redirect = (RedirectResult)result;
        Assert.That(redirect.Url, Is.EqualTo("/Admin/Inquiries"));
    }

    [Test]
    public async Task Login_Post_With_Invalid_Creds_Returns_View_And_Error()
    {
        var controller = new AccountController();

        var result = await controller.Login("x", "y", "/");

        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(controller.ViewBag.Error, Is.Not.Null);
    }

    // Minimal stub to satisfy SignInAsync
    private class ServiceProviderStub : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(IAuthenticationService))
                return new FakeAuthService();
            return null;
        }
    }

    private class FakeAuthService : IAuthenticationService
    {
        public Task SignInAsync(HttpContext context, string? scheme, System.Security.Claims.ClaimsPrincipal principal, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
            => Task.FromResult(AuthenticateResult.NoResult());

        public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;
    }
}
