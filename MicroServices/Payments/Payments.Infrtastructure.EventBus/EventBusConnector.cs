using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Consumers;

namespace Payments.Infrastructure.EventBus
{
    public static class EventBusConnector
    {
        public static void ConnectEventBus(this IServiceCollection services, string hostName)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddConsumersFromNamespaceContaining<GeneratePaymentSessionConsumer>();

                cfg.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(hostName);
                    configurator.ConfigureEndpoints(context);
                });
            });
        }
    }
}
