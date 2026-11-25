using Comiclysm.Api.Models;

public class PublicationDetails
{
    public int ProductId {get; set;}
    public string Publisher {get; set;} = string.Empty;
    public string? Writer {get; set;}
    public string? Artist {get; set;}
    public string? ISBN {get; set;}
    public int? PageCount {get; set;}
    public int? ComicIssueId {get; set;}
    public string? VariantName {get; set;}
    public int PrintingNumber {get; set;} = 1;
    public Product? Product {get; set; }
    public ComicIssue? ComicIssue {get; set;}
 }