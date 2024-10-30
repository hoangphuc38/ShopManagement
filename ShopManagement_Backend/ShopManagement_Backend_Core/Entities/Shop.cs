using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Core.Entities
{
    public class Shop
    {
        public int ShopId { get; set; }

        public int UserId { get; set; }

        public string ShopName { get; set; } = null!;

        public string? ShopAddress { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public Boolean IsDeleted { get; set; }

        public virtual ICollection<ShopDetail> ShopDetails { get; set; } = new List<ShopDetail>();

        public virtual User User { get; set; } = null!;
    }
}
