using Comiclysm.Api.DTOs;
using Comiclysm.Api.Models;
using Comiclysm.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Comiclysm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComicsController : ControllerBase
{
    private readonly IComicInventoryService _inventoryService;

    public ComicsController(
        IComicInventoryService inventoryService
    )
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<ComicInventoryResponse>> GetAll(
        [FromQuery] string? title,
        [FromQuery] string? writer,
        [FromQuery] DateOnly? releaseDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25)
    {

        if (page < 1 || pageSize is < 1 or > 100)
        {
            return BadRequest(
                "Page must be atleast 1 and pageSize must be between 1 and 100."
            );
        }

        var comics = _inventoryService.GetAll(
            title,
            writer,
            releaseDate,
            page,
            pageSize
        );

        return Ok(comics);
    }

    [HttpGet("{id:int}")]
    public ActionResult<ComicInventoryResponse> GetById(int id)
    {
        var comic = _inventoryService.GetById(id);

        if (comic is null)
        {
            return NotFound();
        }

        return Ok(comic);
    }

    [HttpPost]
    public ActionResult<ComicInventoryResponse> Create(
        CreateComicIssuesRequest request
    )
    {
        var comic = _inventoryService.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = comic.ComicIssueId },
            comic
        );
    }

    [HttpPost("{id:int}/receive")]
    public ActionResult<ComicInventoryResponse> ReceiveStock(
        int id,
        AdjustInventoryRequest request
    )
    {

        var comic = _inventoryService.ReceiveStock(id, request.Quantity);

        if (comic is null)
        {
            return NotFound();
        }

        return Ok(comic);
    }

    [HttpPost("{id:int}/sell")]
    public ActionResult<ComicInventoryResponse> SellStock(
        int id,
        AdjustInventoryRequest request
    )
    {
        var existingComic = _inventoryService.GetById(id);

        if (existingComic is null)
        {
            return NotFound();
        }

        if (existingComic.QuantityOnHand < request.Quantity)
        {
            return Conflict(new
            {
                message = "Insufficient inventory.",
                quantityAvailable = existingComic.QuantityOnHand,
                quantityRequested = request.Quantity
            });
        }

        var updatedComic = _inventoryService.SellStock(
            id,
            request.Quantity
        );

        return Ok(updatedComic);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _inventoryService.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}