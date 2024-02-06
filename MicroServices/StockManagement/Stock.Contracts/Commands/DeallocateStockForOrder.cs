namespace Stock.Contracts.Commands;

public record DeallocateStockForOrder(Guid OrderId);