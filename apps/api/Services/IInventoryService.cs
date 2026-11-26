using Comiclysm.Api.DTOs;

namespace Comiclysm.Api.Services;

public interface IInventoryService
{
    Task<IReadOnlyCollection<InventoryResponse>> GetAllAsync(
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