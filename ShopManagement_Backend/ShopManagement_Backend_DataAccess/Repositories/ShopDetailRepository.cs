using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class ShopDetailRepository : BaseRepository<ShopDetail>, IShopDetailRepository
    {
        public ShopDetailRepository(ShopManagementDbContext context) : base(context) { }
    }
}
