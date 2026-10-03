using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.category;

public class UpdateCategoryDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int? ParentId { get; set; }
}