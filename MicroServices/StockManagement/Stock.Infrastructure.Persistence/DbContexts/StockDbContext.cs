using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.Persistence.Entities;

namespace Stock.Infrastructure.Persistence.DbContexts
{
    public class StockDbContext : DbContext
    {
        public const string DefaultSchema = "StockItems";
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Allocation> Allocations{ get; set; }
        public StockDbContext()
        {

        }

        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=db;Database=master;User=sa;Password=Password123;",
                    x => x.MigrationsHistoryTable("__MigrationsHistory", DefaultSchema));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);
            modelBuilder.Entity<Allocation>().HasOne(a => a.Item).WithMany(i => i.Allocations);
        }

    }
}
