namespace Stock.Contracts.Models;

public record StockDetails(
    string Sku,
    string Name,
    int AvailableQuantity,
    string RevenueCode,
    decimal Price
    );