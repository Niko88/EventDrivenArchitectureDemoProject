using MassTransit;
using Orders.Contracts.Commands;
using Orders.Infrastructure.Persistence.Entities;
using Stock.Contracts.Commands;
using Stock.Contracts.Models;

namespace Orders.Infrastructure.Persistence.Sagas.Activities;

public class AllocateStockActivity : IStateMachineActivity<Order, InitiateOrderCommand>
{
    public async Task Execute(BehaviorContext<Order, InitiateOrderCommand> context, IBehavior<Order, InitiateOrderCommand> next)
    {
        await context.Publish(new AllocateStockItemCommand(
            context.Saga.CorrelationId,
            new StockToAllocateDetails(
                context.Message.OrderDetails.ItemCode,
                context.Message.OrderDetails.Quantity
        )));

        await next.Execute(context).ConfigureAwait(false);
    }

    #region Boilerplate

    public void Probe(ProbeContext context)
    {
        context.CreateScope("allocate-stock");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<Order, InitiateOrderCommand, TException> context, IBehavior<Order, InitiateOrderCommand> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    #endregion
}