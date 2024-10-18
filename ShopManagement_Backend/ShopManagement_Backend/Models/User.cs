using System;
using System.Collections.Generic;

namespace ShopManagement_Backend.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Address { get; set; }

    public DateOnly? SignUpDate { get; set; }

    public Boolean IsDeleted { get; set; }

    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
}
