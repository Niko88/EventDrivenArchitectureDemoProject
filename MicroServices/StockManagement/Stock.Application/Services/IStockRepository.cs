using Stock.Contracts.Models;

namespace Stock.Application.Services;

public interface IStockRepository
{
    public Task<(bool IsSuccess, string Error)> AllocateItem(Guid orderId, StockToAllocateDetails details);
    public Task RemoveAllocationForOrder(Guid orderId);
    public Task<IEnumerable<StockDetails>> GetAvailableStock();
}