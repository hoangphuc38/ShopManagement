namespace ShopManagement_Backend_Application.Models.Shop
{
    public class ShopResponse
    {
        public int ShopID { get; set; }

        public string ShopName { get; set; } = null!;

        public string? ShopAddress { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public string OwnerName { get; set; } = null!;
    }
}
