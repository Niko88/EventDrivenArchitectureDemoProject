using MassTransit;
using Orders.Infrastructure.Entities;
using Payments.Contracts.Events;
using Stock.Contracts.Commands;
using Stock.Contracts.Events;

namespace Orders.Infrastructure.Sagas.Activities;

public class DeallocateStockActivity :
    IStateMachineActivity<Order, AllocationFailed>,
    IStateMachineActivity<Order, SessionGenerationErrorEvent>,
    IStateMachineActivity<Order, PaymentFailedEvent>
{
    public async Task Execute(BehaviorContext<Order, AllocationFailed> context, IBehavior<Order, AllocationFailed> next)
    {
        await context.Publish(new DeallocateStockForOrder(context.Saga.CorrelationId));

        await next.Execute(context).ConfigureAwait(false);
    }

    public async Task Execute(BehaviorContext<Order, SessionGenerationErrorEvent> context, IBehavior<Order, SessionGenerationErrorEvent> next)
    {
        await context.Publish(new DeallocateStockForOrder(context.Saga.CorrelationId));

        await next.Execute(context).ConfigureAwait(false);
    }

    public async Task Execute(BehaviorContext<Order, PaymentFailedEvent> context, IBehavior<Order, PaymentFailedEvent> next)
    {
        context.Saga.PaymentStatus = "Fail";
        context.Saga.Message = "Payment failed";
        context.Saga.LastUpdated = DateTime.Now;

        await context.Publish(new DeallocateStockForOrder(context.Saga.CorrelationId));

        await next.Execute(context).ConfigureAwait(false);
    }

    #region BoilerPlate

    public void Probe(ProbeContext context)
    {
        context.CreateScope("deallocate-stock");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<Order, AllocationFailed, TException> context, IBehavior<Order, AllocationFailed> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<Order, SessionGenerationErrorEvent, TException> context, IBehavior<Order, SessionGenerationErrorEvent> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<Order, PaymentFailedEvent, TException> context, IBehavior<Order, PaymentFailedEvent> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    #endregion

}