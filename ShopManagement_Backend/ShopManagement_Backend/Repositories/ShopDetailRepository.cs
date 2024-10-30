using ShopManagement_Backend.Models;
using ShopManagement_Backend.Repositories.Interfaces;

namespace ShopManagement_Backend.Repositories
{
    public class ShopDetailRepository : BaseRepository<ShopDetail>, IShopDetailRepository
    {
        public ShopDetailRepository(ShopManagementDbContext context) : base(context) { }
    }
}
