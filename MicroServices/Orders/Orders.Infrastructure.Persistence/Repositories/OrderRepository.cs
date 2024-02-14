using Microsoft.EntityFrameworkCore;
using Orders.Application.Repositories;
using Orders.Contracts.Models;
using Orders.Infrastructure.Persistence.DBContexts;

namespace Orders.Infrastructure.Persistence.Repositories;

public class OrderRepository(OrdersContext dbContext) : IOrderRepository
{
    public async Task<OrderStatus?> GetOrderStatus(Guid orderId)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.CorrelationId == orderId);
        return new OrderStatus(order.ItemCode, order.State, order.PaymentStatus);
    }
}