using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class ShopRepository : BaseRepository<Shop>, IShopRepository
    {
        public ShopRepository(ShopManagementDbContext context) : base(context) { }
    }
}
