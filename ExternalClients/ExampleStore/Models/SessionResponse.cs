using System.Text.Json.Serialization;

namespace ExampleStore.Models;

public class SessionResponse
{
    [JsonPropertyName("redirectUrl")]
    public string RedirectUrl { get; set; }

    [JsonPropertyName("errors")]
    public string Errors { get; set; }
}