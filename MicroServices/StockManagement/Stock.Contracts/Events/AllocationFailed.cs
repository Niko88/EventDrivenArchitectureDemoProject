using MassTransit;

namespace Stock.Contracts.Events;

public record AllocationFailed(
    Guid CorrelationId, 
    string? Error
) : CorrelatedBy<Guid>;