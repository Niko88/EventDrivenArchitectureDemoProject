using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Consumers;

namespace Payments.Application;

public static class ApplicationConnector
{
    public static void ConnectApplicationLayer(this IServiceCollection services, string hostName)
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