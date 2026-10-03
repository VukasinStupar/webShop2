using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.image;

public class UpdateImageDto
{
    [Required]
    [Url]
    [StringLength(500)]
    public string Url { get; set; } = string.Empty;

    public bool IsMain { get; set; }

    [Range(0, int.MaxValue)]
    public int SortOrder { get; set; }
}