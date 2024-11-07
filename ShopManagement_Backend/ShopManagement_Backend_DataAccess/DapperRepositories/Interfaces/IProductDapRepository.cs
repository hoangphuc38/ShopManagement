using ShopManagement_Backend_Core.Entities;
using static Dapper.SqlMapper;

namespace ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces
{
    public interface IProductDapRepository : IBaseDapRepository<Product>
    {
    }
}
