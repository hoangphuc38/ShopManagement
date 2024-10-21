namespace ShopManagement_Backend.Requests
{
    public class UserRequest
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Address { get; set; } = string.Empty;
    }
}
