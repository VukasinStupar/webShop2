using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class Payment : Entity
    {
        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "RSD";

        public string Provider { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public string? TransactionId { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? FailureReason { get; set; }
    }
}
