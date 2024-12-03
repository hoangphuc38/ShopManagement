using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetProductById(object id);

        Task<IEnumerable<Product>> GetProductsWithPagination(string columnName, string typeSort, string filter);
    }
}
