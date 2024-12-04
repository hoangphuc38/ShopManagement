using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IShopDetailRepository : IBaseRepository<ShopDetail>
    {
        IEnumerable<ShopDetail> GetShopDetailWithPagination(
            object id,
            int page, int pageSize, string sort, string filter, out int total);
    }
}
