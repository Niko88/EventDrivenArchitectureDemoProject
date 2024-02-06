using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.Entities;
using Orders.Infrastructure.EntityConfiguration;

namespace Orders.Infrastructure.DBContexts
{
    public class OrdersContext : DbContext
    {
        public const string DefaultSchema = "Orders";

        public DbSet<Order> Orders { get; set; }

        public OrdersContext()
        {

        }

        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
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
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        }
    }
}
