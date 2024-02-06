
using MassTransit;

namespace Payments.Contracts.Events
{
    public record SessionGenerationErrorEvent(
        Guid CorrelationId,
        bool IsSuccess,
        int? PaymentId,
        string ErrorMessage
    ) : CorrelatedBy<Guid>;
}
