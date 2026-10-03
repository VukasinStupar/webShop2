namespace webShop2.dto.productValue
{
    public class ProductValueDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public string? Text { get; set; }
        public int? Number { get; set; }
        public decimal? Decimal { get; set; }
        public bool? Bool { get; set; }
    }
}
