using MassTransit;
using Stock.Application.Services;
using Stock.Contracts.Commands;

namespace Stock.Application.Consumers;

public class DeallocateStockForOrderConsumer(IStockRepository stockRepository) : IConsumer<DeallocateStockForOrder>
{
    public async Task Consume(ConsumeContext<DeallocateStockForOrder> context)
    {
        await stockRepository.RemoveAllocationForOrder(context.Message.OrderId);
    }
}