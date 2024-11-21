using System;
using System.Collections.Generic;

namespace ShopManagement_Backend_Core.Entities;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Address { get; set; }

    public DateOnly? SignUpDate { get; set; }

    public bool IsDeleted { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
}
