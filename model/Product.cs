using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Status { get; set; } = "Active";

        public int CategoryId { get; set; }
    }
}
