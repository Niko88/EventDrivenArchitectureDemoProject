namespace Orders.Contracts.Models
{
    public record OrderDetails(
        string ItemCode,
        string CustomerCode,
        int Quantity,
        decimal Price,
        string RedirectUrl,
        string RevenueChannel,
        string RevenueCode
    );
}