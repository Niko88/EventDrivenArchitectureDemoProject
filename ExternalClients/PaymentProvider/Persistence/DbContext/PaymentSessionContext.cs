using Microsoft.EntityFrameworkCore;

namespace PaymentProvider.Persistence.DbContext;

public class PaymentSessionContext : Microsoft.EntityFrameworkCore.DbContext
{
    public virtual DbSet<Session> Sessions{ get; set; }
    public PaymentSessionContext()
    {
        
    }

    public PaymentSessionContext(DbContextOptions<PaymentSessionContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("name=RuleDB");
        }
    }
}