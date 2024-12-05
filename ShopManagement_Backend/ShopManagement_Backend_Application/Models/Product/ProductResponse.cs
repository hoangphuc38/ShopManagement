namespace ShopManagement_Backend_Application.Models.Product
{
    public class ProductResponse
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ImageUrl { get; set; }

        public double? Price { get; set; }
    }
}
