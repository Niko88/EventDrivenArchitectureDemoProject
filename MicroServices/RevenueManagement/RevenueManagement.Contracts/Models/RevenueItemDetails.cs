namespace RevenueManagement.Contracts.Models;

public record RevenueItemDetails(
    Guid OrderId, 
    string RevenueCode, 
    string RevenueChannel,
    Guid TransactionId,
    decimal Amount
 );