using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using TransferApp.Data;
using TransferApp.Security; 
using TransferApp.Services;

var builder = WebApplication.CreateBuilder(args);
{
    //var hasher = new PasswordHasher<string>();

  //  var h1 = hasher.HashPassword("admin1", "KostaS1970");
    //var h2 = hasher.HashPassword("admin2", "CSKA@1948");

    //var path = Path.Combine(builder.Environment.ContentRootPath, "admin-hashes.txt");
    //var sb = new StringBuilder();
    //sb.AppendLine("ADMIN1 HASH:");
    //sb.AppendLine(h1);
   // sb.AppendLine();
   // sb.AppendLine("ADMIN2 HASH:");
   // sb.AppendLine(h2);

    //File.WriteAllText(path, sb.ToString());
    //Console.WriteLine("Wrote hashes to: " + path);
}


// Needed by cookie consent feature / Razor
builder.Services.AddHttpContextAccessor();

// EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Email
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services
    .AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// Admin users from config
builder.Services.Configure<AdminUsersOptions>(builder.Configuration.GetSection("AdminUsersOptions"));
builder.Services.AddSingleton<Microsoft.AspNetCore.Identity.PasswordHasher<AdminUser>>();

// Auth (Cookies)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Denied";

        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;

        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // на hosting ще си е https
        options.Cookie.IsEssential = true;
    });

builder.Services.AddAuthorization();

builder.Services.Configure<TransferApp.Options.AdminUsersOptions>(
    builder.Configuration.GetSection("AdminUsersOptions"));


var app = builder.Build();


// Error handling + security headers
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Localization middleware
var supportedCultures = new[] { "bg", "en", "ru", "fr" }
    .Select(c => new CultureInfo(c))
    .ToList();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("bg"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Cookie policy + consent (преди Auth)
app.UseCookiePolicy(new CookiePolicyOptions
{
    CheckConsentNeeded = _ => true,
    MinimumSameSitePolicy = SameSiteMode.Lax,
    Secure = CookieSecurePolicy.SameAsRequest
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }

