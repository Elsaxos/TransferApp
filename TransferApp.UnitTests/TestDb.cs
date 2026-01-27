using Microsoft.EntityFrameworkCore;
using TransferApp.Data;

namespace TransferApp.UnitTests.TestHelpers;

public static class TestDb
{
    public static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var db = new ApplicationDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }
}
