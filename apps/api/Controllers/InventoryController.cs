using Comiclysm.Api.DTOs;
using Comiclysm.Api.Models;
using Comiclysm.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Comiclysm.Api.Controllers;

[ApiController]
[Route("api/stores/{storeId:int}/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(
        IInventoryService inventoryService
    )
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<InventoryResponse>>> GetAll(
        int storeId,
        [FromQuery] string? search,
        [FromQuery] string? productType,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25
    )
    {
        if (page is < 1 or > 1_000_000 || pageSize is < 1 or > 100)
        {
            return BadRequest("Invalid Pagination Parameters.");
        }

        var inventory = await _inventoryService.GetAllAsync(
            storeId,
            search,
            productType,
            page,
            pageSize
        );

        return Ok(inventory);
        
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InventoryResponse>> GetById(
        int storeId,
        int id
    )
    {
        var item = await _inventoryService.GetByIdAsync(storeId, id);
        
        if(item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost("{id:int}/receive")]
    public async Task <ActionResult<InventoryResponse>> ReceiveStock(
        int storeId,
        int id,
        AdjustInventoryRequest request
    )
    {

        var success = await _inventoryService.ReceiveStockAsync(storeId, id, request.Quantity);
        var item = await _inventoryService.GetByIdAsync(storeId, id);

        if(item is null)
        {
            return NotFound();
        }

        if (!success)
        {
            return Conflict("Unable to receive requested quantity.");
        }

        return Ok(item);
    }

    [HttpPost("{id:int}/sell")]
    public async Task<ActionResult<InventoryResponse>> SellStock(
        int storeId,
        int id,
        AdjustInventoryRequest request
    )
    {
        var success = await _inventoryService.SellStockAsync(storeId, id, request.Quantity);
        var item = await _inventoryService.GetByIdAsync(storeId, id);

        if(item is null)
        {
            return NotFound();
        }

        if (!success)
        {
            return Conflict(new
            {
                message = "Insufficient available inventory.",
                quantityAvailable = item.QuantityAvailable,
                quantityRequested = request.Quantity
            });
        }

        return Ok(item);
    }
}