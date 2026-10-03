using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.productValue
{
    public class CreateProductValueDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int ProductAttributeId { get; set; }

        public string? Text { get; set; }

        public int? Number { get; set; }

        public decimal? Decimal { get; set; }

        public bool? Bool { get; set; }
    }
}