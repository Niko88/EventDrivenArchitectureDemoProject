using MassTransit;
using Orders.Contracts.Models;

namespace Orders.Contracts.Commands
{
    public record InitiateOrderCommand(
        Guid CorrelationId,
        OrderDetails OrderDetails
    ) : CorrelatedBy<Guid>;
}
