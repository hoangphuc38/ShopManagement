using System.ComponentModel.DataAnnotations;

namespace ShopManagement_Backend_Application.Models.ShopDetail
{
    public class ShopDetailRequest
    {
        [Required]
        public int ShopId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int? Quantity { get; set; }
    }
}
