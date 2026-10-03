using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.payment
{
    public class CreatePaymentDto
    {
        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Provider { get; set; } = string.Empty;
    }
}