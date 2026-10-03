using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class Item : Entity
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
