using Microsoft.EntityFrameworkCore;
using Payments.Infrastructure.Entities;

namespace Payments.Infrastructure.DbContexts
{
    public class PaymentsContext : DbContext
    {
        public const string DefaultSchema = "Payments";
        public DbSet<Payment> Payments { get; set; }
        public PaymentsContext()
        {

        }

        public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options)
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
}
