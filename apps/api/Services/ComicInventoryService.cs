using Comiclysm.Api.DTOs;
using Comiclysm.Api.Models;

namespace Comiclysm.Api.Services;

public class ComicInventoryService : IComicInventoryService
{
    private readonly List<ComicSeries> _series = [];
    private readonly List<ComicIssue> _issues = [];
    private readonly List<InventoryItem> _inventory = [];

    private int _nextSeriesId = 1;
    private int _nextIssueId = 1;

    private int _nextInventoryId = 1;


    public IReadOnlyCollection<ComicInventoryResponse> GetAll(
        string? title,
        string? writer,
        DateOnly? releaseDate,
        int page,
        int pageSize
    )
    {
        var query = from issue in _issues
                    join series in _series
                        on issue.ComicSeriesId equals series.Id
                    join inventory in _inventory
                        on issue.Id equals inventory.ComicIssueId
                    select MapToResponse(issue, series, inventory);


        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(comic => comic.SeriesTitle.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(writer))
        {
            query = query.Where(comic => comic.Writer.Contains(writer, StringComparison.OrdinalIgnoreCase));
        }

        if (releaseDate.HasValue)
        {
            query = query.Where(comic => comic.ReleaseDate == releaseDate.Value);
        }

        return query
        .OrderBy(comic => comic.SeriesTitle)
        .ThenBy(comic => comic.IssueNumber)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();
    }

    public ComicInventoryResponse? GetById(int comicIssueId)
    {
        var issue = _issues.FirstOrDefault(issue => issue.Id == comicIssueId);

        if (issue is null)
        {
            return null;
        }

        var series = _series.FirstOrDefault(series => series.Id == issue.ComicSeriesId);

        var inventory = _inventory.FirstOrDefault(item => item.ComicIssueId == comicIssueId);

        if (series is null || inventory is null)
        {
            return null;
        }

        return MapToResponse(issue, series, inventory);
    }

    public ComicInventoryResponse Create(CreateComicIssuesRequest request)
    {
        var series = _series.FirstOrDefault(existingSeries => existingSeries.Title.Equals(request.SeriesTitle, StringComparison.OrdinalIgnoreCase) && existingSeries.Publisher.Equals(request.Publisher, StringComparison.OrdinalIgnoreCase));

        if (series is null)
        {
            series = new ComicSeries
            {
                Id = _nextSeriesId++,
                Title = request.SeriesTitle.Trim(),
                Publisher = request.Publisher.Trim(),
                IsActive = true
            };

            _series.Add(series);
        }

        var issue = new ComicIssue
        {
            Id = _nextIssueId++,
            ComicSeriesId = series.Id,
            IssueNumber = request.IssueNumber,
            Writer = request.Writer.Trim(),
            Artist = request.Artist?.Trim(),
            ReleaseDate = request.ReleaseDate,
            CoverPrice = request.CoverPrice
        };

        var inventory = new InventoryItem
        {
            Id = _nextInventoryId++,
            ComicIssueId = issue.Id,
            QuantityOnHand = request.InitialQuantity,
            ReorderThreshold = request.ReorderThreshold,
            ShelfLocation = request.ShelfLocation?.Trim()
        };

        _issues.Add(issue);
        _inventory.Add(inventory);

        return MapToResponse(issue, series, inventory);
    }

    public ComicInventoryResponse? ReceiveStock(
        int comicIssueId,
        int quantity
    )
    {
        var inventory = _inventory.FirstOrDefault(
            item => item.ComicIssueId == comicIssueId
        );

        if (inventory is null)
        {
            return null;
        }

        inventory.QuantityOnHand += quantity;

        return GetById(comicIssueId);
    }

    public ComicInventoryResponse? SellStock(
        int comicIssueId,
        int quantity
    )
    {
        var inventory = _inventory.FirstOrDefault(
            item => item.ComicIssueId == comicIssueId
        );

        if (inventory is null || inventory.QuantityOnHand < quantity)
        {
            return null;
        }

        inventory.QuantityOnHand -= quantity;

        return GetById(comicIssueId);
    }

    public bool Delete(int comicIssueId)
    {
        var issue = _issues.FirstOrDefault(
            issue => issue.Id == comicIssueId
        );


        if (issue is null)
        {
            return false;
        }

        var inventory = _inventory.FirstOrDefault(item => item.ComicIssueId == comicIssueId);

        if (inventory is not null)
        {
            _inventory.Remove(inventory);
        }

        _issues.Remove(issue);

        return true;
    }


    private static ComicInventoryResponse MapToResponse(
        ComicIssue issue,
        ComicSeries series,
        InventoryItem inventory
    )
    {
        return new ComicInventoryResponse
        {
            ComicIssueId = issue.Id,
            SeriesTitle = series.Title,
            IssueNumber = issue.IssueNumber,
            Writer = issue.Writer,
            Artist = issue.Artist,
            Publisher = series.Publisher,
            ReleaseDate = issue.ReleaseDate,
            CoverPrice = issue.CoverPrice,
            QuantityOnHand = inventory.QuantityOnHand,
            ShelfLocation = inventory.ShelfLocation
        };
    }
}