using System;
using System.Collections.Generic;

namespace ShopManagement_Backend_Core.Entities;

public class Token
{
    public int TokenId { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateOnly? ExpiredDate { get; set; }
}
