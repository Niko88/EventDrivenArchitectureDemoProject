using MassTransit;
using Stock.Application.Services;
using Stock.Contracts.Commands;
using Stock.Contracts.Events;

namespace Stock.Application.Consumers
{
    public class AllocateStockItemsConsumer(IStockRepository stockRepository) : IConsumer<AllocateStockItemCommand>
    {
        public async Task Consume(ConsumeContext<AllocateStockItemCommand> context)
        {
            var allocationResult = await stockRepository.AllocateItem(context.Message.OrderId, context.Message.ItemDetails);

            if (allocationResult.IsSuccess)
            {
                await context.Publish(new AllocationCompletedSuccessfully(context.Message.OrderId));
            }
            else
            {
                await context.Publish(new AllocationFailed(context.Message.OrderId, allocationResult.Error));
            }
        }
    }
}
