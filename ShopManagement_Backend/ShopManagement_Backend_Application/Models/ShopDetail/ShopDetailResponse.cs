using ShopManagement_Backend_Application.Models.Product;

namespace ShopManagement_Backend_Application.Models.ShopDetail
{
    public class ShopDetailResponse
    {
        public ProductResponse Product { get; set; } = null!;

        public int? Quantity { get; set; }
    }
}
