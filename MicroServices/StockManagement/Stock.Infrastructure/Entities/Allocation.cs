namespace Stock.Infrastructure.Persistence.Entities;

public class Allocation
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public int ItemId { get; set; }
    public InventoryItem Item { get; set; }
    public int AllocatedQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}