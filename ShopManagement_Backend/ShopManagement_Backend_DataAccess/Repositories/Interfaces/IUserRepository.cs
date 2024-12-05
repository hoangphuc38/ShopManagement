using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByShopID(int shopID);

        IEnumerable<User>? GetUsersWithPagination(
            int page, int pageSize, string sort, string filter, out int total);
    }
}
