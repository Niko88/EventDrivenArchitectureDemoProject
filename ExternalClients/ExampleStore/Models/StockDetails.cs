using System.Text.Json.Serialization;

namespace ExampleStore.Models;

public class StockDetails
{
    [JsonPropertyName("sku")]
    public string Sku { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("availableQuantity")]
    public int AvailableQuantity { get; set; }

    [JsonPropertyName("revenueCode")]
    public string RevenueCode { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}