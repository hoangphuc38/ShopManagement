using System.ComponentModel.DataAnnotations;

namespace ShopManagement_Backend_Application.Models.Product
{
    public class ProductRequest
    {
        [Required]
        public string? ProductName { get; set; }

        [Required]
        public double? Price { get; set; }
    }
}
