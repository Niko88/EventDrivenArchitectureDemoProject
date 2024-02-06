using Microsoft.EntityFrameworkCore;
using RevenueManagement.Infrastructure.Persistence.Entities;

namespace RevenueManagement.Infrastructure.Persistence.DbContexts;

public class RevenueItemsContext : DbContext
{
    public const string DefaultSchema = "RevenueItems";
    public DbSet<RevenueItem> RevenueItems { get; set; }
    public RevenueItemsContext()
    {

    }

    public RevenueItemsContext(DbContextOptions<RevenueItemsContext> options) : base(options)
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
    }
}