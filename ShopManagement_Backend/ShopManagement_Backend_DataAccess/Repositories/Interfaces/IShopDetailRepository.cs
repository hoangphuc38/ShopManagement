using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IShopDetailRepository : IBaseRepository<ShopDetail>
    {
        IEnumerable<ShopDetail> GetAllAsyncByShopID(object id);
    }
}
