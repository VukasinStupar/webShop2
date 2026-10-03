using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class Image : Entity
    {
        public int ProductId { get; set; }

        public string Url { get; set; } = string.Empty;

        public bool IsMain { get; set; }

        public int SortOrder { get; set; }

    }
}
