namespace webShop2.dto.image
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public int SortOrder { get; set; }
    }
}
