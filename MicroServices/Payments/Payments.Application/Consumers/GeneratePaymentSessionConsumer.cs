using MassTransit;
using Payments.Application.Repositories;
using Payments.Application.Services;
using Payments.Contracts.Commands;
using Payments.Contracts.Events;

namespace Payments.Application.Consumers
{
    public class GeneratePaymentSessionConsumer(IPaymentsRepository paymentsRepository, IPaymentProviderService paymentProvider) : IConsumer<GeneratePaymentSessionCommand>
    {
        public async Task Consume(ConsumeContext<GeneratePaymentSessionCommand> context)
        {
            var (isSuccess, sessionUrl, paymentId) = await GeneratePaymentSession(context.Message);

            if (isSuccess)
            {
                await context.Publish(new SessionGeneratedEvent
                (
                    context.Message.CorrelationId,
                    isSuccess,
                    paymentId,
                    sessionUrl
                ));
            }
            else
            {
                await context.Publish(new SessionGenerationErrorEvent
                (
                    context.Message.CorrelationId,
                    isSuccess,
                    paymentId,
                    sessionUrl
                ));
            }
        }

        private async Task<(bool IsSuccess, string Result, int? paymentId)> GeneratePaymentSession(GeneratePaymentSessionCommand command)
        {
            var (paymentCreated, paymentId) = await paymentsRepository.StorePaymentDetailsAsync(command.PaymentDetails, command.CustomerCode);

            if (!paymentCreated)
                return (false, "A Payment could not be created", null);

            var sessionResponse = await paymentProvider.CreateSession(command, paymentId);

            return sessionResponse.ProviderResponseWasSuccessful ?
                (true, sessionResponse.PaymentSessionUrl, paymentId) : 
                (false, "A Payment session could not be generated", paymentId);
        }
    }
}
