using MassTransit;
using Payments.Application.Repositories;
using Payments.Contracts.Commands;
using Payments.Contracts.Events;

namespace Payments.Application.Consumers;

public class UpdatePaymentStatusConsumer(IPaymentsRepository repository): IConsumer<UpdatePaymentStatusCommand>
{
    public async Task Consume(ConsumeContext<UpdatePaymentStatusCommand> context)
    {
        var paymentDetails = await repository.UpdatePaymentStatus(context.Message.NotificationContent);

        if (paymentDetails is not null)
        {
            if (context.Message.NotificationContent.IsSuccess)
            {
                await context.Publish(new PaymentSucceededEvent(Guid.Parse(paymentDetails.OrderId), paymentDetails.TransactionId.Value));
            }
            else
            {
                await context.Publish(new PaymentFailedEvent(Guid.Parse(paymentDetails.OrderId)));
            }
        }
    }
}