using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TransferApp.Data;
using TransferApp.Models;

namespace TransferApp.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly bool _authenticate;
    private SqliteConnection? _connection;

    public CustomWebApplicationFactory(bool authenticate = false)
    {
        _authenticate = authenticate;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
           
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            
            if (_authenticate)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

                services.AddAuthorization();
            }

            
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            db.Database.EnsureCreated();
            Seed(db);
        });
    }

    private static void Seed(ApplicationDbContext db)
    {
        
        if (!db.PriceItems.Any())
        {
            db.PriceItems.AddRange(
                new PriceItem
                {
                    RouteKey = "sofia-plovdiv",
                    RouteBg = "София → Пловдив",
                    RouteEn = "Sofia → Plovdiv",
                    RouteRu = "София → Пловдив",
                    RouteFr = "Sofia → Plovdiv",
                    OneWayPrice = 100m,
                    RoundTripPrice = 180m,
                    IsActive = true,
                    SortOrder = 1
                },
                new PriceItem
                {
                    RouteKey = "sofia-varna",
                    RouteBg = "София → Варна",
                    RouteEn = "Sofia → Varna",
                    RouteRu = "София → Варна",
                    RouteFr = "Sofia → Varna",
                    OneWayPrice = 220m,
                    RoundTripPrice = 400m,
                    IsActive = true,
                    SortOrder = 2
                }
            );

            db.SaveChanges();
        }

        
        if (!db.TransferRequests.Any())
        {
            db.TransferRequests.AddRange(
                new TransferRequest
                {
                    CustomerName = "Ivan Ivanov",
                    Phone = "+359888111222",
                    Email = "ivan@example.com",
                    PickupAddress = "Sofia Center",
                    DropoffAddress = "Plovdiv Center",
                    PickupDateTime = DateTime.UtcNow.AddDays(2),
                    Passengers = 2,
                    Price = 100m,
                    Status = "Запитване",
                    Notes = "Test seed 1"
                },
                new TransferRequest
                {
                    CustomerName = "Maria Petrova",
                    Phone = "+359888333444",
                    Email = "maria@example.com",
                    PickupAddress = "Sofia Airport",
                    DropoffAddress = "Varna",
                    PickupDateTime = DateTime.UtcNow.AddDays(5),
                    Passengers = 3,
                    Price = 220m,
                    Status = "Резервация",
                    Notes = "Test seed 2"
                }
            );

            db.SaveChanges();
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection?.Dispose();
            _connection = null;
        }
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    ) : base(options, logger, encoder)
    { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Name, "Test User"),

          
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "Administrator"),
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}



