using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetProductById(object id);

        IEnumerable<Product>? GetProductsWithPagination(
            int page, int pageSize, string typeSort, string filter, out int total);
    }
}
