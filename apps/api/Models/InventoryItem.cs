namespace Comiclysm.Api.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public int ComicIssueId { get; set; }
    public int QuantityOnHand { get; set; }
    public int ReorderThreshold { get; set; }
    public string? ShelfLocation { get; set; }
    public ComicIssue? ComicIssue { get; set; }
}