using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.item;

public class CreateItemDto
{
    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}