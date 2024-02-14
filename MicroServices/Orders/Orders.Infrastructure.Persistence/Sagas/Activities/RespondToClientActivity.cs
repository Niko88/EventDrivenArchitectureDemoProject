using MassTransit;
using Orders.Contracts.PublishedEvents;
using Orders.Infrastructure.Persistence.Entities;
using Payments.Contracts.Events;
using Stock.Contracts.Events;

namespace Orders.Infrastructure.Persistence.Sagas.Activities;

public class RespondToClientActivity :
    IStateMachineActivity<Order, SessionGeneratedEvent>,
    IStateMachineActivity<Order, SessionGenerationErrorEvent>,
    IStateMachineActivity<Order, AllocationFailed>
{
    public async Task Execute(BehaviorContext<Order, SessionGeneratedEvent> context, IBehavior<Order, SessionGeneratedEvent> next)
    {
        context.Saga.PaymentId = context.Message.PaymentId;
        context.Saga.Message = "Session Url generated";
        context.Saga.LastUpdated = DateTime.Now;

        var responseEndpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress);

        await responseEndpoint.Send(new OrderGenerationResult(
                CorrelationId: context.Saga.CorrelationId,
                IsSuccess: context.Message.IsSuccess,
                SessionUrl: context.Message.SessionUrl,
                PaymentId: context.Message.PaymentId
            ),
            r => r.RequestId = context.Saga.RequestId);

        await next.Execute(context).ConfigureAwait(false);

    }

    public async Task Execute(BehaviorContext<Order, SessionGenerationErrorEvent> context, IBehavior<Order, SessionGenerationErrorEvent> next)
    {
        context.Saga.PaymentId = context.Message.PaymentId;
        context.Saga.Message = context.Message.ErrorMessage;
        context.Saga.LastUpdated = DateTime.Now;

        var responseEndpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress);

        await responseEndpoint.Send(new OrderGenerationResult(
                CorrelationId: context.Saga.CorrelationId,
                IsSuccess: context.Message.IsSuccess,
                ErrorMessages: context.Message.ErrorMessage,
                PaymentId: context.Message.PaymentId
            ),
            r => r.RequestId = context.Saga.RequestId);

        await next.Execute(context).ConfigureAwait(false);
    }

    public async Task Execute(BehaviorContext<Order, AllocationFailed> context, IBehavior<Order, AllocationFailed> next)
    {
        context.Saga.Message = "Stock allocation failed";
        context.Saga.LastUpdated = DateTime.Now;

        var responseEndpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress);

        await responseEndpoint.Send(new OrderGenerationResult(
                CorrelationId: context.Saga.CorrelationId,
                IsSuccess: false,
                ErrorMessages: context.Message.Error
            ),
            r => r.RequestId = context.Saga.RequestId);

        await next.Execute(context).ConfigureAwait(false);
    }

    #region Boilerplate

    public void Probe(ProbeContext context)
    {
        context.CreateScope("respond-to-client");
    }
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<Order, SessionGeneratedEvent, TException> context, IBehavior<Order, SessionGeneratedEvent> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<Order, SessionGenerationErrorEvent, TException> context, IBehavior<Order, SessionGenerationErrorEvent> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<Order, AllocationFailed, TException> context, IBehavior<Order, AllocationFailed> next) where TException : Exception
    {
        await next.Faulted(context);
    }
    #endregion
}