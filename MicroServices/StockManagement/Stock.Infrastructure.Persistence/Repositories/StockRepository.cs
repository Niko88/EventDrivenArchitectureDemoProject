using Microsoft.EntityFrameworkCore;
using Stock.Application.Services;
using Stock.Contracts.Models;
using Stock.Infrastructure.Persistence.DbContexts;
using Stock.Infrastructure.Persistence.Entities;

namespace Stock.Infrastructure.Persistence.Repositories;

public class StockRepository(StockDbContext dbContext) : IStockRepository
{
    public async Task<(bool IsSuccess, string Error)> AllocateItem(Guid orderId, StockToAllocateDetails details)
    {
        try
        {
            var item = await dbContext.InventoryItems
                .Where(i => i.Sku == details.Sku)
                .Include(ii => ii.Allocations)
                .FirstOrDefaultAsync();

            if (item == null)
                return (false, $"We could not find this item {details.Sku}");

            var allocations = item.Allocations.ToList();

            allocations.Add(new Allocation
            {
                OrderId = orderId,
                AllocatedQuantity = details.Quantity,
                LastUpdatedDate = DateTime.Now
            });

            var updatedQuantity = item.AvailableQuantity -= details.Quantity;

            if (updatedQuantity <= 0)
                return (false, $"We do not have enough {item.Name} for your order");

            item.AvailableQuantity = updatedQuantity;
            item.Allocations = allocations;
            await dbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return (false, $"We could not reserve this item {exception.Message}");
        }

        return (true, string.Empty);
    }

    public async Task RemoveAllocationForOrder(Guid orderId)
    {
        var allocations = dbContext.Allocations.Where(a => a.OrderId == orderId).Include(a => a.Item);
        foreach (var allocation in allocations)
        {
            var item = allocation.Item;
            item.AvailableQuantity += allocation.AllocatedQuantity;
            dbContext.Update(item);
        }

        dbContext.RemoveRange(allocations);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<StockDetails>> GetAvailableStock()
    {
        var items = await dbContext.InventoryItems.ToListAsync();

        return items.Select(i => new StockDetails(
            i.Sku, 
            i.Name,
            i.AvailableQuantity, 
            i.RevenueCode,
            i.Price
        ));
    }
}