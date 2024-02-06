using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.DBContexts;
using Orders.Infrastructure.Entities;

namespace Orders.Infrastructure.Repositories;

public class OrderRepository(OrdersContext dbContext) : IOrderRepository
{
    public async Task<Order?> GetOrder(Guid orderId)
    {
        return await dbContext.Orders.FirstOrDefaultAsync(o => o.CorrelationId == orderId);
    }
}

public interface IOrderRepository
{
    public Task<Order?> GetOrder(Guid orderId);
}