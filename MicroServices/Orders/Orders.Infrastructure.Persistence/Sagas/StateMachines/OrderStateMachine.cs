using MassTransit;
using Orders.Contracts.Commands;
using Orders.Infrastructure.Entities;
using Orders.Infrastructure.Sagas.Activities;
using Payments.Contracts.Events;
using RevenueManagement.Contracts.Commands;
using RevenueManagement.Contracts.Models;
using Stock.Contracts.Events;

namespace Orders.Infrastructure.Sagas.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<Order>
    {
        #region States
        public State AllocatingOrderItem { get; private set; } = null!;
        public State GeneratingPaymentSession { get; private set; } = null!;
        public State PaymentPending { get; private set; } = null!;
        public State PaymentSucceeded { get; private set; } = null!;
        public State OrderFailed { get; private set; } = null!;

        #endregion

        #region Events

        public Event<InitiateOrder> InitiateOrder { get; private set; } = null!;
        public Event<AllocationCompletedSuccessfully> AllocationSuccessReceived { get; private set; } = null!;
        public Event<AllocationFailed> AllocationErrorReceived { get; private set; } = null!;
        public Event<SessionGeneratedEvent> SessionGenerationSuccessReceived { get; private set; } = null!;
        public Event<SessionGenerationErrorEvent> SessionGenerationErrorReceived { get; private set; } = null!;
        public Event<PaymentSucceededEvent> PaymentResultSuccessfulResponseReceived { get; private set; } = null!;
        public Event<PaymentFailedEvent> PaymentResultFailedResponseReceived { get; private set; } = null!;

        #endregion

        public OrderStateMachine()
        {

            InstanceState(x => x.State);
            StoreOrderInfo();


            Initially(
                When(InitiateOrder)
                    .Activity(x => x.OfType<AllocateStockActivity>())
                    .TransitionTo(AllocatingOrderItem)
            );

            During(AllocatingOrderItem,
                When(AllocationErrorReceived)
                    .Activity(x => x.OfType<RespondToClientActivity>())
                    .TransitionTo(OrderFailed)
            );

            During(AllocatingOrderItem,
                When(AllocationSuccessReceived)
                    .Activity(x => x.OfType<GeneratePaymentSessionActivity>())
                    .TransitionTo(GeneratingPaymentSession)
            );

            During(GeneratingPaymentSession,
                When(SessionGenerationErrorReceived)
                    .Activity(x => x.OfType<DeallocateStockActivity>())
                    .Activity(x => x.OfType<RespondToClientActivity>())
                    .TransitionTo(OrderFailed)
            );

            During(GeneratingPaymentSession,
                When(SessionGenerationSuccessReceived)
                    .Activity(x => x.OfType<RespondToClientActivity>())
                    .TransitionTo(PaymentPending)
            );

            During(PaymentPending,
                When(PaymentResultFailedResponseReceived)
                    .Activity(x => x.OfType<DeallocateStockActivity>())
                    .TransitionTo(OrderFailed)
            );

            During(PaymentPending,
                When(PaymentResultSuccessfulResponseReceived)
                    .TransitionTo(PaymentSucceeded)
                    .ThenAsync(context =>
                    {
                        context.Saga.PaymentStatus = "Success";
                        context.Saga.Message = "Order paid";
                        context.Saga.TransactionId = context.Message.TransactionId;
                        context.Saga.LastUpdated = DateTime.Now;

                        return context.Publish(new RecordRevenueCommand(new RevenueItemDetails(
                            context.Saga.CorrelationId,
                            context.Saga.RevenueCode,
                            context.Saga.RevenueChannel,
                            context.Message.TransactionId,
                            context.Saga.Price
                        )));
                    })
                    .Finalize()
            );
        }

        //This method creates an entry on the state machine mapping our initial request to the Payment instance (needs to be called at the beginning of the saga right after setting the state)
        private void StoreOrderInfo()
        {
            Event(() => InitiateOrder, e =>
            {
                e.CorrelateById(context => context.Message.CorrelationId);
                e.InsertOnInitial = true;
                e.SetSagaFactory(context => new Order()
                {
                    CorrelationId = context.Message.CorrelationId,
                    CustomerCode = context.Message.OrderDetails.CustomerCode,
                    Description = "New Order",
                    RedirectUrl = $"{context.Message.OrderDetails.RedirectUrl}{context.Message.CorrelationId}",
                    ItemCode = context.Message.OrderDetails.ItemCode,
                    Quantity = context.Message.OrderDetails.Quantity,
                    Price = context.Message.OrderDetails.Price,
                    State = Initial.Name,
                    PaymentStatus = "Unknown",
                    Message = "Starting new order",
                    RevenueChannel = context.Message.OrderDetails.RevenueChannel,
                    RevenueCode = context.Message.OrderDetails.RevenueCode,
                    CreatedDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    ResponseAddress = context.ResponseAddress,
                    RequestId = context.RequestId,
                });
            });
        }
    }
}
