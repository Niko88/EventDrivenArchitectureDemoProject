using System.Text.Json.Serialization;

namespace ExampleStore.Models;

public class StockResponse
{
    [JsonPropertyName("stockItems")]
    public IEnumerable<StockDetails> StockItems { get; set; }

}