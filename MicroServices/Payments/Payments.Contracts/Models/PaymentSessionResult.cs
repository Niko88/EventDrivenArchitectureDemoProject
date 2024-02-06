namespace Payments.Contracts.Models;

public class PaymentSessionResult(bool providerResponseWasSuccessful, string paymentSessionUrl)
{
    public bool ProviderResponseWasSuccessful { get; set; } = providerResponseWasSuccessful;
    public string PaymentSessionUrl{ get; set; } = paymentSessionUrl;
}