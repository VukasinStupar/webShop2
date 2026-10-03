using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class Order : Entity
    {
        public string Number { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string CustomerPhone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
