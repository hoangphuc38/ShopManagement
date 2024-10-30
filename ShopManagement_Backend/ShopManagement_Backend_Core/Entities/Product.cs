using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public double? Price { get; set; }

        public Boolean IsDeleted { get; set; }

        public virtual ICollection<ShopDetail> ShopDetails { get; set; } = new List<ShopDetail>();
    }
}
