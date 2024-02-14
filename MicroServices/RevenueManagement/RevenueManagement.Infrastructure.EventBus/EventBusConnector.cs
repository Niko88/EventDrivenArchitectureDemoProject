using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RevenueManagement.Application.Consumers;

namespace RevenueManagement.Infrastructure.EventBus
{
    public static class EventBusConnector
    {
        public static void ConnectEventBus(this IServiceCollection services, string hostName)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddConsumersFromNamespaceContaining<RecordRevenueConsumer>();
                cfg.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(hostName);
                    configurator.ConfigureEndpoints(context);
                });
            });
        }
    }
}
