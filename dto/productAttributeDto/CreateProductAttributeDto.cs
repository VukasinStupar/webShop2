using System.ComponentModel.DataAnnotations;
using webShop2.model;

namespace webShop2.dto.productAttribute
{
    public class CreateProductAttributeDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public AttributeType Type { get; set; }
    }
}