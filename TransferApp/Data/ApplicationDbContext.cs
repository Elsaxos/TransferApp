using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TransferApp.Models;

namespace TransferApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TransferRequest> TransferRequests { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Driver> Drivers { get; set; }
    }
}
