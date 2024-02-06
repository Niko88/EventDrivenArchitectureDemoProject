using MassTransit;

namespace Payments.Contracts.Events
{
    public record PaymentSucceededEvent(Guid CorrelationId, Guid TransactionId) : CorrelatedBy<Guid>;
}
