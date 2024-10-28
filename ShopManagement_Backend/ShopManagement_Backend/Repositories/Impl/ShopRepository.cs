using ShopManagement_Backend.Models;

namespace ShopManagement_Backend.Repositories.Impl
{
    public class ShopRepository : BaseRepository<Shop>, IShopRepository
    {
        public ShopRepository(ShopManagementDbContext context) : base(context) { }
    }
}
