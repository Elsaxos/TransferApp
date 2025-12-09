using Microsoft.EntityFrameworkCore;
using TransferApp.Data;
using TransferApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрираме MVC
builder.Services.AddControllersWithViews();

// Регистрираме EF Core с нашия ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрираме имейл услугата (можем и да не я ползваме още)
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

var app = builder.Build();

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

