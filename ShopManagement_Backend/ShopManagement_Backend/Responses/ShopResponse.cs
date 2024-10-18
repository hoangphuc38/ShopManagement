namespace ShopManagement_Backend.Responses
{
    public class ShopResponse
    {
        public string ShopName { get; set; } = null!;

        public string? ShopAddress { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public string OwnerName { get; set; } = null!;
    }
}
