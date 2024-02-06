using RevenueManagement.Application.Services;
using RevenueManagement.Contracts.Models;
using RevenueManagement.Infrastructure.Persistence.DbContexts;
using RevenueManagement.Infrastructure.Persistence.Entities;

namespace RevenueManagement.Infrastructure.Persistence.Repositories;

public class RevenueItemsRepository(RevenueItemsContext dbContext) : IRevenueItemsRepository
{
    public async Task AddRevenueItem(RevenueItemDetails details)
    {
        var newRevenueItem = new RevenueItem
        {
            OrderId = details.OrderId,
            RevenueCode = details.RevenueCode,
            RevenueChannel = details.RevenueChannel,
            TransactionId = details.TransactionId,
            Amount = details.Amount,
            CreatedDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now
        };
        await dbContext.RevenueItems.AddAsync(newRevenueItem);
        await dbContext.SaveChangesAsync();
    }
}