using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TransferApp.Data;
using TransferApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрираме EF Core с нашия ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрираме имейл услугата (можем и да не я ползваме още)
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// MVC + локализация за View-та и DataAnnotations
builder.Services
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

var app = builder.Build();

// Настройки за поддържаните езици
var supportedCultures = new[]
{
    new CultureInfo("bg"),
    new CultureInfo("en"),
    new CultureInfo("ru"),
    new CultureInfo("fr")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("bg"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

// позволяваме смяна на езика чрез query string и cookie
localizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
localizationOptions.RequestCultureProviders.Insert(1, new CookieRequestCultureProvider());

app.UseRequestLocalization(localizationOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
