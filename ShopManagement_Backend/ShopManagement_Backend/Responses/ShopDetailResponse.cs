using ShopManagement_Backend.Models;

namespace ShopManagement_Backend.Responses
{
    public class ShopDetailResponse
    {
        public ProductResponse Product { get; set; } = null!;

        public int? Quantity { get; set; }
    }
}
