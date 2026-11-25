using Microsoft.AspNetCore.SignalR;

namespace Comiclysm.Api.Models;

public class ComicIssue
{
    public int Id { get; set; }
    public int ComicSeriesId { get; set; }
    public int IssueNumber { get; set; }
    public ComicSeries? ComicSeries { get; set; }
}