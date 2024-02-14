using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Consumers;

namespace Stock.Infrastructure.EventBus
{
    public static class EventBusConnector
    {
        public static void ConnectEventBus(this IServiceCollection services, string hostName)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddConsumersFromNamespaceContaining<AllocateStockItemsConsumer>();
                cfg.AddConsumersFromNamespaceContaining<DeallocateStockForOrderConsumer>();

                cfg.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(hostName);
                    configurator.ConfigureEndpoints(context);
                });
            });
        }
    }
}
