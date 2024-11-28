using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IShopRepository : IBaseRepository<Shop>
    {
        Task<IEnumerable<Shop>> GetAllShops();
    }
}
