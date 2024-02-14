using MassTransit;
using Orders.Application.Repositories;
using Orders.Contracts.Queries;

namespace Orders.Application.Consumers
{
    public class GetOrderStatusConsumer(IOrderRepository orderRepository) : IConsumer<GetOrderStatusQuery>
    {
        public async Task Consume(ConsumeContext<GetOrderStatusQuery> context)
        {
            var orderStatus = await orderRepository.GetOrderStatus(context.Message.OrderId);
            await context.RespondAsync(orderStatus);
        }
    }
}
