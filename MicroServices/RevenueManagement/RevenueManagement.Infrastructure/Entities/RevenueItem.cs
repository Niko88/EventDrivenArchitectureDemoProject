namespace RevenueManagement.Infrastructure.Persistence.Entities;

public class RevenueItem
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public string RevenueCode { get; set; }
    public string RevenueChannel { get; set; }
    public Guid TransactionId { get; set; }
    public decimal Amount{ get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}