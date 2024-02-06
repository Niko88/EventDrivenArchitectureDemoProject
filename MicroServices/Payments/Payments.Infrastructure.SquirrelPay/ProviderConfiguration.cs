namespace Payments.Infrastructure.SquirrelPay;

public class ProviderConfiguration
{
    public string BaseAddress { get; set; }
    public string NotificationUrl { get; set; }
    public string CreateSessionEndpoint { get; set; }
    public string ClientName { get; set; }
}