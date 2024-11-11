using System.ComponentModel.DataAnnotations;

namespace ShopManagement_Backend_Application.Models.User
{
    public class UserRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        public string Address { get; set; } = string.Empty;
    }
}
