namespace Comiclysm.Api.DTOs;

public class InventoryResponse
{
    public int Id { get; set; }
    public int StoreId {get; set;}
    public int ProductId {get; set;}
    public string ProductName {get; set;} = string.Empty;
    public string ProductType {get; set; } = string.Empty;
    
    public string? ImageUrl {get; set;}
    public decimal RetailPrice{get; set;}
    public decimal SalePrice {get; set;}
    public int QuantityOnHand {get; set;}
    public int QuantityReserved{get;set;}
    public int QuantityAvailable {get; set;}
    public int ReorderThreshold {get; set;}
    public string? ShelfLocation {get; set;}
    public string? Publisher {get; set;}
    public string? Writer {get; set;}
    public DateOnly? ReleaseDate {get; set;}
}