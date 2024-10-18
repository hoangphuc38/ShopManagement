using System;
using System.Collections.Generic;

namespace ShopManagement_Backend.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public double? Price { get; set; }

    public Boolean IsDeleted { get; set; }

    public virtual ICollection<ShopDetail> ShopDetails { get; set; } = new List<ShopDetail>();
}
