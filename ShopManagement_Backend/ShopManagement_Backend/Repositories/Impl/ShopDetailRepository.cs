using ShopManagement_Backend.Models;

namespace ShopManagement_Backend.Repositories.Impl
{
    public class ShopDetailRepository : BaseRepository<ShopDetail>, IShopDetailRepository
    {
        public ShopDetailRepository(ShopManagementDbContext context) : base(context) { }
    }
}
