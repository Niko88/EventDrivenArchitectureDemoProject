using System.Net.Http.Json;
using System.Text.Json;
using Payments.Application.Services;
using Payments.Contracts.Commands;
using Payments.Contracts.Models;
using Payments.Infrastructure.SquirrelPay.Contracts;

namespace Payments.Infrastructure.SquirrelPay.Services;

public class PaymentProviderService(IHttpClientFactory clientFactory, ProviderConfiguration configurationOptions) : IPaymentProviderService
{
    public async Task<PaymentSessionResult> CreateSession(GeneratePaymentSessionCommand command, int paymentId)
    {
        try
        {
            using var httpClient = clientFactory.CreateClient(configurationOptions.ClientName);
            var response = await httpClient.PostAsJsonAsync(configurationOptions.CreateSessionEndpoint, new GenerateSessionRequestModel(
                PaymentId: paymentId,
                Amount: command.PaymentDetails.Price,
                NotifyUrl: configurationOptions.NotificationUrl,
                RedirectUrl: command.RedirectUrl
            ));

            var responseContent = await response.Content.ReadAsStringAsync();
            var generatedSessionResponse = JsonSerializer.Deserialize<GenerateSessionResponse>(responseContent);
            return new PaymentSessionResult(true, generatedSessionResponse.SessionUrl);
        }
        catch (Exception exception)
        {
            return new PaymentSessionResult(false, exception.Message);
        }
    }
}