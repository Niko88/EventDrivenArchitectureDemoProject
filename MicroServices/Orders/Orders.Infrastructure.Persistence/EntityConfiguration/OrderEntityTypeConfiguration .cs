using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Infrastructure.DBContexts;
using Orders.Infrastructure.Entities;

namespace Orders.Infrastructure.EntityConfiguration
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
