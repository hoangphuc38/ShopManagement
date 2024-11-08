using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces
{
    public interface IShopDetailDapRepository
    {
        int AddAsync(ShopDetail entity);

        int DeleteAsync(ShopDetail entity);

        int UpdateAsync(ShopDetail entity);

        IEnumerable<ShopDetail> GetAllAsyncByProductID(object id);

        IEnumerable<ShopDetail> GetAllAsyncByShopID(object id);
    }
}
