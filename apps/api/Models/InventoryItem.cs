namespace Comiclysm.Api.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    // QuantityOnHand - # of copies available for the public (non-reserved copies).
    public int QuantityOnHand { get; set; }
    // QuantityReserved - # of copies allocated for customers with a "pull list" (subscribers).
    public int QuantityReserved { get; set; }
    public int ReorderThreshold { get; set; }
    public decimal SalePrice { get; set; }
    public string? ShelfLocation { get; set; }
    public Store? Store { get; set; }
    public Product? Product { get; set; }
    public int QuantityAvailable => QuantityOnHand - QuantityReserved;
}