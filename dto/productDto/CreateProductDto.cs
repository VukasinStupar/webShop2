using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.ProductDto;

public class CreateProductDto
{
    [Required]
    [StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 2)]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}