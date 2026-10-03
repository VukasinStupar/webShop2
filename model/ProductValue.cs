using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class ProductValue : Entity
    {
        public int ProductId { get; set; }

        public int ProductAttributeId { get; set; }

        public string? Text { get; set; }

        public int? Number { get; set; }

        public decimal? Decimal { get; set; }

        public bool? Bool { get; set; }
    }
}
