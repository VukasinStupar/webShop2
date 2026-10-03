using System.ComponentModel.DataAnnotations;

namespace webShop2.dto.payment
{
    public class UpdatePaymentDto
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Status { get; set; } = string.Empty;

        [StringLength(150)]
        public string? TransactionId { get; set; }

        public DateTime? PaidAt { get; set; }

        [StringLength(500)]
        public string? FailureReason { get; set; }
    }
}