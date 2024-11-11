using System.ComponentModel.DataAnnotations;

namespace ShopManagement_Backend_Application.Models.Shop
{
    public class ShopRequest
    {
        [Required]
        public string? ShopName { get; set; }

        [Required]
        public string? ShopAddress { get; set; }
    }
}
