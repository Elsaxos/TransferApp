using Microsoft.EntityFrameworkCore;
using TransferApp.Models;

namespace TransferApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TransferRequest> TransferRequests { get; set; } = default!;
        public DbSet<Car> Cars { get; set; } = default!;
        public DbSet<Driver> Drivers { get; set; } = default!;
        public DbSet<PriceItem> PriceItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ важни precision-и за пари (за да няма предупреждения/рязане)
            modelBuilder.Entity<PriceItem>(e =>
            {
                e.Property(x => x.OneWayPrice).HasPrecision(18, 2);
                e.Property(x => x.RoundTripPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<TransferRequest>(e =>
            {
                e.Property(x => x.Price).HasPrecision(18, 2);
            });
        }
    }
}
