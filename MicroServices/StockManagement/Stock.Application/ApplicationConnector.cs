using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Consumers;

namespace Stock.Application
{
    public static class ApplicationConnector
    {
        public static void ConnectApplicationLayer(this IServiceCollection services, string hostName)
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
