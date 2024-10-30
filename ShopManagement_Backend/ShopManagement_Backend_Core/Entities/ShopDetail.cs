using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Core.Entities
{
    public class ShopDetail
    {
        public int ShopId { get; set; }

        public int ProductId { get; set; }

        public int? Quantity { get; set; }

        public Boolean IsDeleted { get; set; }

        public virtual Product Product { get; set; } = null!;

        public virtual Shop Shop { get; set; } = null!;
    }
}
