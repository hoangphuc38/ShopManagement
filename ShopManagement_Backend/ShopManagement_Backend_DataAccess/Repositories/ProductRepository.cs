using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Constants;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Net.WebSockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public IEnumerable<Product> GetProductsWithPagination(
            int page, int pageSize,
            string columnName, string typeSort, string filter, out int total)
        {
            try
            {
                _logger.LogInformation($"[GetProductsWithPagination] Start to connect to db");

                using var connection = Context.GetDbConnection();

                string sortOrder = typeSort == SortType.Ascending ? SortType.AscendingSort : SortType.DescendingSort;

                int skipRows = (page - 1) * pageSize;

                string sqlQuery =
                    @"SELECT ProductID, ProductName, Price 
                      FROM PRODUCT
                      WHERE IsDeleted = 0 " + filter +
                      " ORDER BY " + columnName + " " + sortOrder +
                      " OFFSET " + skipRows + " ROWS " +
                      " FETCH NEXT " + pageSize +" ROWS ONLY;" +
                      " " +
                      "SELECT COUNT(ProductID) " +
                      "FROM Product " +
                      "WHERE IsDeleted = 0";

                using (var multi = connection.QueryMultiple(sqlQuery))
                {
                    total = multi.ReadSingle();
                    var productList = multi.Read<Product>().ToList();

                    return productList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetProductsWithPagination] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
