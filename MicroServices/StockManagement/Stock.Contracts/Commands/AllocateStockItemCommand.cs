using Stock.Contracts.Models;

namespace Stock.Contracts.Commands
{
    public record AllocateStockItemCommand(
        Guid OrderId,
        StockToAllocateDetails ItemDetails
    );
}
