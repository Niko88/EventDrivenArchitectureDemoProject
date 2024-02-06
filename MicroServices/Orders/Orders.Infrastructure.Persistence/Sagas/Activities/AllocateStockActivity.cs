using MassTransit;
using Orders.Contracts.Commands;
using Orders.Infrastructure.Entities;
using Stock.Contracts.Commands;
using Stock.Contracts.Models;

namespace Orders.Infrastructure.Sagas.Activities;

public class AllocateStockActivity : IStateMachineActivity<Order, InitiateOrder>
{
    public async Task Execute(BehaviorContext<Order, InitiateOrder> context, IBehavior<Order, InitiateOrder> next)
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

    public async Task Faulted<TException>(BehaviorExceptionContext<Order, InitiateOrder, TException> context, IBehavior<Order, InitiateOrder> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    #endregion
}