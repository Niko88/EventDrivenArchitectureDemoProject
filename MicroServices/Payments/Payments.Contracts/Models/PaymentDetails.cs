namespace Payments.Contracts.Models;

public record PaymentDetails(
    string OrderId, 
    decimal Price, 
    string? SessionId = null, 
    Guid? TransactionId = null
);