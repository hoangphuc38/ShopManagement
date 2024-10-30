using ShopManagement_Backend.Models;
using ShopManagement_Backend.Repositories.Interfaces;

namespace ShopManagement_Backend.Repositories
{
    public class ShopRepository : BaseRepository<Shop>, IShopRepository
    {
        public ShopRepository(ShopManagementDbContext context) : base(context) { }
    }
}
