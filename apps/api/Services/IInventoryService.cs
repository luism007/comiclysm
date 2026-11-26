using Comiclysm.Api.DTOs;

namespace Comiclysm.Api.Services;

public interface IInventoryService
{
    IReadOnlyCollection<InventoryResponse> GetAllSync(
        int storeId,
        string? search,
        string? productType,
        int page,
        int pageSize
    );

    Task<InventoryResponse?> GetByIdAsync(
        int storeId,
        int inventoryId
    );

    Task<bool> ReceiveStockAsync(
        int storeId,
        int inventoryId,
        int quantity
    );

    Task<bool> SellStockAsync(
        int storeId,
        int inventoryId,
        int quantity
    );
}