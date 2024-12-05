namespace ShopManagement_Backend_API.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RoleAttribute : Attribute
    {
        public string? Roles { get; set; }
    }
}
