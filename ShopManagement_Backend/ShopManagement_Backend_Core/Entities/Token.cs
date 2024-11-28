using System;
using System.Collections.Generic;

namespace ShopManagement_Backend_Core.Entities;

public class Token
{
    public int TokenId { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? ExpiredDate { get; set; }

    public int UserID { get; set; }
}
