using System.ComponentModel.DataAnnotations;

namespace Comiclysm.Api.DTOs;

public class CreateComicIssuesRequest
{
    [Required]
    public string SeriesTitle { get; set; } = string.Empty;

    [Required]
    public string Publisher { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int IssueNumber { get; set; }

    [Required]
    public string Writer { get; set; } = string.Empty;

    public string? Artist { get; set; }
    public DateOnly ReleaseDate { get; set; }

    [Range(0, double.MaxValue)]
    public decimal CoverPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int InitialQuantity { get; set; }

    public int ReorderThreshold { get; set; }

    public string? ShelfLocation { get; set; }
}