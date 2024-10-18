using System;
using System.Collections.Generic;

namespace ShopManagement_Backend.Models;

public partial class ShopDetail
{
    public int ShopId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public Boolean IsDeleted { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Shop Shop { get; set; } = null!;
}
