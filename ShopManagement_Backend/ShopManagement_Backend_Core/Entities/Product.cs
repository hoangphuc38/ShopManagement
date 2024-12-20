﻿using System;
using System.Collections.Generic;

namespace ShopManagement_Backend_Core.Entities;

public class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ImageUrl { get; set; }

    public double? Price { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ShopDetail> ShopDetails { get; set; } = new List<ShopDetail>();
}
