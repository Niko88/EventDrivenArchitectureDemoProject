using RevenueManagement.Contracts.Models;

namespace RevenueManagement.Application.Services
{
    public interface IRevenueItemsRepository
    {
        Task AddRevenueItem(RevenueItemDetails details);
    }
}
