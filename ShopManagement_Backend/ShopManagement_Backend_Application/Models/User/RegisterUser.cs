using System.ComponentModel.DataAnnotations;

namespace ShopManagement_Backend_Application.Models.User
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Enter your FullName!")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Enter your UserName!")]
        [EmailAddress(ErrorMessage = "Invalid UserName!")]
        public string? UserName { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Enter your Password!")]
        public string? Password { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "Enter your Role!")]
        public string? Role {  get; set; }
    }
}
