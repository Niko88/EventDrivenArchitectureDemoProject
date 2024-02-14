namespace Stock.Infrastructure.Persistence.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    public string Sku{ get; set; }
    public string Name{ get; set; }
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
    public string RevenueCode{ get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public IEnumerable<Allocation> Allocations { get; set; }
}