using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.item;

public class UpdateItemDto
{
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}