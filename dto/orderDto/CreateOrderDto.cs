using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.order;

public class CreateOrderDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [StringLength(30, MinimumLength = 5)]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string PostalCode { get; set; } = string.Empty;
}