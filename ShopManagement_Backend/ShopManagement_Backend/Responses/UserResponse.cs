namespace ShopManagement_Backend.Responses
{
    public class UserResponse
    {
        public int UserID { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateOnly? SignUpDate { get; set; }
    }
}
