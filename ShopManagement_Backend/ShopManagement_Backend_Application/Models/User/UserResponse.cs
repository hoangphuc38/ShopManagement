﻿namespace ShopManagement_Backend_Application.Models.User
{
    public class UserResponse
    {
        public int UserID { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateTime? SignUpDate { get; set; }
    }
}
