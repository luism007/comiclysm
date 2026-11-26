using Comiclysm.Api.Data;
using Comiclysm.Api.DTOs;
using Comiclysm.Api.Models;
using Comiclysm.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Comiclysm.Api.Services;

public class InventoryService : IInventoryService
{
   private readonly ComiclysmDbContext _context;

   public InventoryService(ComiclysmDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<InventoryResponse>> GetAllAsync(
        int storeId,
        string? search,
        string? productType,
        int page,
        int pageSize
    )
    {
        var query = _context.InventoryItems
        .AsNoTracking() // Disable Entity Change Tracking since its a Read-Only Query
        .Include(item => item.Product)
            .ThenInclude(product => product!.PublicationDetails)
        .Where(item => item.StoreId == storeId);

        if(!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToUpper();
            query = query.Where(item => item.Product!.Name.ToUpper().Contains(term));
        }

        if(!string.IsNullOrWhiteSpace(productType))
        {
            if(!Enum.TryParse<ProductType>(productType, true, out var type) 
            || !Enum.IsDefined(type))
            {
                return [];
            }

            query = query.Where(item => item.Product!.Type == type);
        }

        var items = await query
            .OrderBy(item => item.Product!.Name)
            .ThenBy(item => item.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return items.Select(MapToResponse).ToList();
    }

    public async Task<InventoryResponse?> GetByIdAsync(
        int storeId, 
        int inventoryId
    )
    {
        var item = await _context.InventoryItems
        .AsNoTracking()
        .Include(item => item.Product)
            .ThenInclude(product => product!.PublicationDetails)
        .FirstOrDefaultAsync(item =>
            item.StoreId == storeId && item.Id == inventoryId
        );

        return item is null ? null : MapToResponse(item);
    }

    public async Task<bool> ReceiveStockAsync(
        int storeId,
        int inventoryId,
        int quantity
    )
    {
        if (quantity <= 0)
        {
            return false;
        }

        var affectedRows = await _context.InventoryItems
        .Where(item => 
            item.StoreId == storeId && 
            item.Id == inventoryId &&
            item.QuantityOnHand <= int.MaxValue - quantity
        )
        .ExecuteUpdateAsync(setters => setters
        .SetProperty(
            item => item.QuantityOnHand,
            item => item.QuantityOnHand + quantity
        ));

        return affectedRows > 0;
    }

    public async Task<bool> SellStockAsync(
        int storeId,
        int inventoryId,
        int quantity
    )
    {
        if (quantity <= 0)
        {
            return false;
        }

        var affectedRows = await _context.InventoryItems
        .Where(item => 
            item.StoreId == storeId &&
            item.Id == inventoryId &&
            item.QuantityOnHand - item.QuantityReserved >= quantity
        )
        .ExecuteUpdateAsync(setters => setters
        .SetProperty(
            item => item.QuantityOnHand,
            item => item.QuantityOnHand - quantity
        ));

        return affectedRows > 0;
    }
    private static InventoryResponse MapToResponse(
        InventoryItem item
    )
    {
        var product = item.Product!;

        return new InventoryResponse
        {
            Id = item.Id,
            StoreId = item.StoreId,
            ProductId = item.ProductId,

            ProductName = product.Name,
            ProductType = product.Type.ToString(),
            ImageUrl = product.ImageUrl,
            RetailPrice = product.RetailPrice,
            ReleaseDate = product.ReleaseDate,

            SalePrice = item.SalePrice,
            QuantityOnHand = item.QuantityOnHand,
            QuantityReserved = item.QuantityReserved,
            QuantityAvailable = item.QuantityAvailable,
            ReorderThreshold = item.ReorderThreshold,
            ShelfLocation = item.ShelfLocation,

            Publisher = product.PublicationDetails?.Publisher,
            Writer = product.PublicationDetails?.Writer

        };
    }
}