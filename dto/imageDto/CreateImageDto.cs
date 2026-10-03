using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.image;

public class CreateImageDto
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Required]
    [Url]
    [StringLength(500)]
    public string Url { get; set; } = string.Empty;

    public bool IsMain { get; set; }

    [Range(0, int.MaxValue)]
    public int SortOrder { get; set; }
}