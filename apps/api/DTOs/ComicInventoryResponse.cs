namespace Comiclysm.Api.DTOs;

public class ComicInventoryResponse
{
    public int ComicIssueId { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public int IssueNumber { get; set; }

    public string Writer { get; set; } = string.Empty;

    public string? Artist { get; set; }
    public string Publisher { get; set; } = string.Empty;

    public DateOnly ReleaseDate { get; set; }

    public decimal CoverPrice { get; set; }

    public int QuantityOnHand { get; set; }

    public string? ShelfLocation { get; set; }
}