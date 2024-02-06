
using MassTransit;

namespace Payments.Contracts.Events
{
    public record SessionGeneratedEvent(
        Guid CorrelationId, bool IsSuccess, 
        int? PaymentId, 
        string SessionUrl
    ) : CorrelatedBy<Guid>;
}
