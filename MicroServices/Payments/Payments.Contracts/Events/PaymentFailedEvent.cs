using MassTransit;

namespace Payments.Contracts.Events
{
    public record PaymentFailedEvent(Guid CorrelationId) : CorrelatedBy<Guid>;
}
