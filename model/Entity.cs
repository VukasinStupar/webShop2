using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
