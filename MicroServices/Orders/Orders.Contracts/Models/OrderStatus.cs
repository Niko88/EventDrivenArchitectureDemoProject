namespace Orders.Contracts.Models;

public record OrderStatus(
    string OrderedItem,
    string OrderState,
    string PaymentStatus
);