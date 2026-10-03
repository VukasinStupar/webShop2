using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class ProductAttribute : Entity
    {
        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public AttributeType Type { get; set; }
    }
}
