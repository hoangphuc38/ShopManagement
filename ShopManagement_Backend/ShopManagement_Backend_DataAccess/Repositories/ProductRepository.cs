using Dapper;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(
            ShopManagementDbContext context,
            ILogger<ProductRepository> logger) : base(context, logger)  { }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation($"[GetAllProduct] Start to connect to db");
                using var connection = Context.GetDbConnection();

                var sqlQuery = "SELECT ProductID, ProductName, Price FROM PRODUCT WHERE IsDeleted = 0";

                var productList = connection.Query<Product>(sqlQuery);

                return productList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllProduct] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
