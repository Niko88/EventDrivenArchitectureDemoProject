using MassTransit;
using Orders.Contracts.Models;

namespace Orders.Contracts.Commands
{
    public record InitiateOrder(
        Guid CorrelationId,
        OrderDetails OrderDetails
    ) : CorrelatedBy<Guid>;
}
