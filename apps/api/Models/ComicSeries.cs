namespace Comiclysm.Api.Models;

public class ComicSeries
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public List<ComicIssue> Issues { get; set; } = [];
}