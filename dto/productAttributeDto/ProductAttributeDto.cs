using webShop2.model;

namespace webShop2.dto.productAttribute
{
    public class ProductAttributeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public AttributeType Type { get; set; }
    }
}
