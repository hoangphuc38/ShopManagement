namespace ShopManagement_Backend.Requests
{
    public class ShopDetailRequest
    {
        public int ShopId { get; set; }

        public int ProductId { get; set; }

        public int? Quantity { get; set; }
    }
}
