public class Product
{
    public int Id {get; set; }
    public string Name {get; set;} = string.Empty;
    public string? Description {get; set;}
    public ProductType Type {get; set;}

    public decimal RetailPrice {get; set;}
    public string? ImageUrl {get; set;}
    public string? Barcode {get; set;}
    public DateOnly? ReleaseDate {get; set;}
    public bool IsActive {get; set; } = true;
}