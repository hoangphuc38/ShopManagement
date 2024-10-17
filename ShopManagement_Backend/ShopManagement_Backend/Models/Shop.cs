using System;
using System.Collections.Generic;

namespace ShopManagement_Backend.Models;

public partial class Shop
{
    public int ShopId { get; set; }

    public int UserId { get; set; }

    public string ShopName { get; set; } = null!;

    public string? ShopAddress { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public virtual ICollection<ShopDetail> ShopDetails { get; set; } = new List<ShopDetail>();

    public virtual User User { get; set; } = null!;
}
