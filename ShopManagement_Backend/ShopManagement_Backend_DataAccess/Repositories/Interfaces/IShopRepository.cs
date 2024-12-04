using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IShopRepository : IBaseRepository<Shop>
    {
        IEnumerable<Shop>? GetShopsWithPagination(
            int page, int pageSize, string sort, string filter, out int total);

        IEnumerable<Shop> GetShopByUserID(
            object userID,
            int page, int pageSize, string sort, string filter, out int total);
    }
}
