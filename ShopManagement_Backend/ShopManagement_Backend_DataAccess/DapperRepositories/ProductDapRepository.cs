using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces;
using ShopManagement_Backend_DataAccess.Persistance;
using static Dapper.SqlMapper;

namespace ShopManagement_Backend_DataAccess.DapperRepositories
{
    public class ProductDapRepository : IProductDapRepository
    {
        protected readonly ShopManagementDapperContext Context;
        protected readonly ILogger<ProductDapRepository> _logger;

        public ProductDapRepository(
            ShopManagementDapperContext context,
            ILogger<ProductDapRepository> logger)
        {
            Context = context;
            _logger = logger;   
        }

        public int AddAsync(Product entity)
        {
            using (var connection = Context.CreateConnection())
            {
                _logger.LogInformation($"[AddAsyncProduct] Start to connect to db");
                var transaction = connection.BeginTransaction();

                var sqlQuery = $"INSERT INTO PRODUCT (ProductName, Price, IsDeleted) " +
                    $"VALUES (@ProductName, @Price, @IsDeleted)";

                try
                {
                    var recordChanges = connection.Execute(sqlQuery, entity);
                    transaction.Commit();

                    return recordChanges;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[AddAsyncProduct] Error: {ex.Message}");
                    transaction.Rollback();
                    return 0;
                }
            }
        }

        public int DeleteAsync(Product entity)
        {
            using (var connection = Context.CreateConnection())
            {
                _logger.LogInformation($"[DeleteAsyncProduct] Start to connect to db");
                var transaction = connection.BeginTransaction();

                var sqlQuery = "UPDATE PRODUCT SET IsDeleted = 1 WHERE ProductId = @ProductId";

                try
                {
                    var recordChanges = connection.Execute(sqlQuery, entity);
                    transaction.Commit();

                    return recordChanges;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[DeleteAsyncProduct] Error: {ex.Message}");
                    transaction.Rollback();
                    return 0;
                }
            }
        }

        public IEnumerable<Product> GetAllAsync()
        {
            try
            {
                _logger.LogInformation($"[GetAllAsyncProduct] Start to connect to db");
                using var connection = Context.CreateConnection();

                var sqlQuery = "SELECT ProductID, ProductName, Price FROM PRODUCT WHERE IsDeleted = 0";

                var productList = connection.Query<Product>(sqlQuery);

                return productList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllAsyncProduct] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public Product? GetFirstOrNullAsync(object id)
        {
            try
            {
                _logger.LogInformation($"[GetFirstOrNullAsyncProduct] Start to connect to db with id: {id}");
                using var connection = Context.CreateConnection();

                var sqlQuery = "SELECT ProductID, ProductName, Price FROM PRODUCT WHERE ProductID = @Id AND IsDeleted = 0";

                var product = connection.QueryFirstOrDefault<Product>(sqlQuery, new { id });

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetFirstOrNullAsyncProduct] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public int UpdateAsync(Product entity)
        {
            using (var connection = Context.CreateConnection())
            {
                _logger.LogInformation($"[UpdateAsyncProduct] Start to connect to db");
                var transaction = connection.BeginTransaction();

                var sqlQuery = $"UPDATE PRODUCT SET ProductName = @ProductName, Price = @Price" +
                    $"WHERE ProductId = @ProductId AND IsDeleted = 0";

                try
                {
                    var recordChanges = connection.Execute(sqlQuery, entity);
                    transaction.Commit();

                    return recordChanges;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[UpdateAsyncProduct] Error: {ex.Message}");
                    transaction.Rollback();
                    return 0;
                }
            }
        }
    }
}
