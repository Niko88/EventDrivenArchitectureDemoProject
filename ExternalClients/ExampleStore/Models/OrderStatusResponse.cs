using System.Text.Json.Serialization;

namespace ExampleStore.Models
{
    public class OrderStatusResponse
    {
        [JsonPropertyName("orderedItem")]
        public string OrderedItem { get; set; }

        [JsonPropertyName("orderStatus")]
        public string OrderStatus { get; set; }

        [JsonPropertyName("paymentStatus")]
        public string PaymentStatus { get; set; }
    }
}
