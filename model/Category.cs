using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class Category : Entity
    {
        public string Name { get; set; } = string.Empty;

        public int? ParentId { get; set; }
    }
}
