using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Infrastructure.Persistence.DBContexts;
using Orders.Infrastructure.Persistence.Entities;

namespace Orders.Infrastructure.Persistence.EntityConfiguration
{
    internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(OrdersContext.Orders), OrdersContext.DefaultSchema);
            builder.HasKey(p => p.CorrelationId);
        }
    }

}
