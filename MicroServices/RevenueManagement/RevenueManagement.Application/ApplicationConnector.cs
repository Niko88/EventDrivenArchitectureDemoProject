using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RevenueManagement.Application.Consumers;

namespace RevenueManagement.Application
{
    public static class ApplicationConnector
    {
        public static void ConnectApplicationLayer(this IServiceCollection services, string hostName)
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
