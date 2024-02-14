using Orders.Contracts.Models;

namespace Orders.Application.Repositories;

public interface IOrderRepository
{
    public Task<OrderStatus> GetOrderStatus(Guid orderId);
}