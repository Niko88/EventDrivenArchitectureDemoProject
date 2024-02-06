using MassTransit;

namespace Stock.Contracts.Events;

public record AllocationCompletedSuccessfully(Guid CorrelationId) : CorrelatedBy<Guid>;