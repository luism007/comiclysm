using Microsoft.AspNetCore.SignalR;

namespace Comiclysm.Api.Models;

public class ComicIssue
{
    public int Id { get; set; }
    public int ComicSeriesId { get; set; }
    public int IssueNumber { get; set; }
    public string Writer { get; set; } = string.Empty;
    public string? Artist { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public decimal CoverPrice { get; set; }
    public string? CoverImageUrl { get; set; }
    public ComicSeries? ComicSeries { get; set; }
    public List<InventoryItem> InventoryItems { get; set; } = [];
}