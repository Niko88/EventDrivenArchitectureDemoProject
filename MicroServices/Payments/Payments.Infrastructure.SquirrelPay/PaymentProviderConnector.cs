using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Services;
using Payments.Infrastructure.SquirrelPay.Services;

namespace Payments.Infrastructure.SquirrelPay;

public static class PaymentProviderConnector
{
    public static void ConnectSquirrelPay(this IServiceCollection services, ProviderConfiguration configuration)
    {
        services.AddSingleton(configuration);

        services.AddTransient<IPaymentProviderService, PaymentProviderService>();

        services.AddHttpClient("squirrelPay", (_, client) =>
        {
            var baseAddress = configuration.BaseAddress;
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });
    }
}