using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class ShopRepository : BaseRepository<Shop>, IShopRepository
    {
        public ShopRepository(
            ShopManagementDbContext context,
            ILogger<ShopRepository> logger) : base(context, logger) { }

        public IEnumerable<Shop> GetAllShops()
        {
            var shopList = Context.Shops.Include(x => x.User).Where(c => !c.IsDeleted).ToList();

            return shopList;
        }
    }
}
