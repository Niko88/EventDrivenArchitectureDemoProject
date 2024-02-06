using MassTransit;
using RevenueManagement.Application.Services;
using RevenueManagement.Contracts.Commands;

namespace RevenueManagement.Application.Consumers;

public class RecordRevenueConsumer(IRevenueItemsRepository revenueItemsRepository) : IConsumer<RecordRevenueCommand>
{
    public async Task Consume(ConsumeContext<RecordRevenueCommand> context)
    {
        await revenueItemsRepository.AddRevenueItem(context.Message.Details);
    }
}