using MassTransit;
using Orders.Infrastructure.Entities;
using Payments.Contracts.Commands;
using Payments.Contracts.Models;
using Stock.Contracts.Events;

namespace Orders.Infrastructure.Sagas.Activities
{
    internal class GeneratePaymentSessionActivity : IStateMachineActivity<Order, AllocationCompletedSuccessfully>
    {
        public async Task Execute(BehaviorContext<Order, AllocationCompletedSuccessfully> context, IBehavior<Order, AllocationCompletedSuccessfully> next)
        {
            var paymentDetails = new PaymentDetails
            (
                Price: context.Saga.Price,
                OrderId: context.Saga.CorrelationId.ToString()
            );

            await context.Publish(new GeneratePaymentSessionCommand(
                context.Saga.CorrelationId,
                context.Saga.CustomerCode,
                paymentDetails,
                context.Saga.Description,
                context.Saga.RedirectUrl
            ));

            await next.Execute(context).ConfigureAwait(false);
        }


        #region Boilerplate

        public void Probe(ProbeContext context)
        {
            context.CreateScope("request-payment");
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Faulted<TException>(BehaviorExceptionContext<Order, AllocationCompletedSuccessfully, TException> context, IBehavior<Order, AllocationCompletedSuccessfully> next) where TException : Exception
        {
            await next.Faulted(context);
        }

        #endregion
    }
}
