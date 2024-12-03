using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Constants;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Net.WebSockets;

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
                    @"SELECT ProductID, ProductName, Price 
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Product>> GetProductsWithPagination(
            string columnName, string typeSort, string filter)
        {
            try
            {
                _logger.LogInformation($"[GetProductsWithPagination] Start to connect to db");

                using var connection = Context.GetDbConnection();

                string sqlQuery = "";
                string sortOrder = typeSort == SortType.Ascending ? SortType.AscendingSort : SortType.DescendingSort;

                sqlQuery =
                    @"SELECT ProductID, ProductName, Price 
                      FROM PRODUCT
                      WHERE IsDeleted = 0 " + filter +
                      " ORDER BY " + columnName + " " + sortOrder;

                var productList = await connection.QueryAsync<Product>(sqlQuery);

                return productList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetProductsWithPagination] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
