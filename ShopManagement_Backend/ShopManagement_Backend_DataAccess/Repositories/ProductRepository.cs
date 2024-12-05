using Dapper;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Constants;
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

        public async Task<Product> GetProductById(object id)
        {
            try
            {
                _logger.LogInformation($"[GetProductByID] Start to connect to db");
                using var connection = Context.GetDbConnection();

                var sqlQuery = 
                    @"SELECT ProductID, ProductName, Price, ImageUrl 
                      FROM PRODUCT 
                      WHERE ProductID = @ProductID AND IsDeleted = 0";

                var parameters = new DynamicParameters();
                parameters.Add("@ProductID", id);

                var product = await connection.QueryFirstAsync<Product>(sqlQuery, parameters);

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetProductByID] Error: {ex.Message}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsWithPagination(
            int page, int pageSize, string sort, string filter, out int total)
        {
            try
            {
                _logger.LogInformation($"[GetProductsWithPagination] Start to connect to db");

                using var connection = Context.GetDbConnection();

                string sqlQuery =
                    $@"SELECT 
		                    ProductID, 
		                    ProductName, 
		                    Price,
                            ImageUrl,
		                    COUNT(ProductID) OVER() AS TotalRecords  
                    FROM PRODUCT
                    WHERE IsDeleted = 0
                    {filter}
                    {sort} OFFSET @skipRows ROWS FETCH NEXT @pageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("@skipRows", (page - 1) * pageSize);
                parameters.Add("@pageSize", pageSize);

                var productList = connection.Query<Product>(sqlQuery, parameters);

                dynamic result = connection.QueryFirst(sqlQuery, parameters);

                total = result.TotalRecords;

                return productList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetProductsWithPagination] Error: {ex.Message}");
                total = 0;
                return new List<Product>();
            }
        }
    }
}
