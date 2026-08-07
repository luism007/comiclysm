using System.ComponentModel.DataAnnotations;

namespace Comiclysm.Api.DTOs;

public class AdjustInventoryRequest
{
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}