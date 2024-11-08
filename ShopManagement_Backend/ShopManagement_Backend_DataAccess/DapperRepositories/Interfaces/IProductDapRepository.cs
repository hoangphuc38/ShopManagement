using ShopManagement_Backend_Core.Entities;
using static Dapper.SqlMapper;

namespace ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces
{
    public interface IProductDapRepository
    {
        int AddAsync(Product entity);

        int DeleteAsync(Product entity);

        int UpdateAsync(Product entity);

        IEnumerable<Product> GetAllAsync();

        Product? GetFirstOrNullAsync(object param);
    }
}
