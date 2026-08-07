using Comiclysm.Api.DTOs;

namespace Comiclysm.Api.Services;

public interface IComicInventoryService
{
    IReadOnlyCollection<ComicInventoryResponse> GetAll(
        string? title,
        string? writer,
        DateOnly? releaseDate,
        int page,
        int pageSize
        );

    ComicInventoryResponse? GetById(int comicIssueId);

    ComicInventoryResponse Create(CreateComicIssuesRequest request);

    ComicInventoryResponse? ReceiveStock(
        int comicIssueId,
        int quantity
    );

    ComicInventoryResponse? SellStock(
        int comicIssueId,
        int quantity
    );

    bool Delete(int comicIssueId);
}