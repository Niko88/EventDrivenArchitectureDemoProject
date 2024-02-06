using System.Text.Json.Serialization;

namespace Payments.Infrastructure.SquirrelPay.Contracts;

public class GenerateSessionResponse
{
    [JsonPropertyName("sessionUrl")]
    public string SessionUrl { get; set; }
}