namespace webShop2.dto.category;

public class CategoryStatsDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalPrice { get; set; }
}