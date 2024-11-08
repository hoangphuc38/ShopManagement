using Dapper;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces;
using ShopManagement_Backend_DataAccess.Persistance;

namespace ShopManagement_Backend_DataAccess.DapperRepositories
{
    public class ShopDetailDapRepository : IShopDetailDapRepository
    {
        protected readonly ShopManagementDapperContext Context;
        protected readonly ILogger<ShopDetailDapRepository> _logger;

        public ShopDetailDapRepository(
            ShopManagementDapperContext context,
            ILogger<ShopDetailDapRepository> logger)
        {
            Context = context;
            _logger = logger;
        }

        public int AddAsync(ShopDetail entity)
        {
            using (var connection = Context.CreateConnection())
            {
                _logger.LogInformation($"[AddAsyncShopDetail] Start to connect to db");
                var transaction = connection.BeginTransaction();

                var sqlQuery = $"INSERT INTO SHOPDETAIL (ShopID, ProductID, Quantity) " +
                    $"VALUES (@ShopID, @ProductID, @Quantity)";

                try
                {
                    var recordChanges = connection.Execute(sqlQuery, entity);
                    transaction.Commit();

                    return recordChanges;
                }
                catch (Exception ex) 
                {
                    _logger.LogError($"[AddAsyncShopDetail] Error: {ex.Message}");
                    transaction.Rollback();                 
                    return 0; 
                }
            }
        }

        public int DeleteAsync(ShopDetail entity)
        {
            using (var connection = Context.CreateConnection())
            {
                _logger.LogInformation($"[DeleteAsyncShopDetail] Start to connect to db");
                var transaction = connection.BeginTransaction();

                var sqlQuery = $"UPDATE SHOPDETAIL SET IsDeleted = 1 " +
                    $"WHERE ShopID = @ShopID AND ProductID = @ProductID";

                try
                {
                    var recordChanges = connection.Execute(sqlQuery, entity);
                    transaction.Commit();

                    return recordChanges;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[DeleteAsyncShopDetail] Error: {ex.Message}");
                    transaction.Rollback();                   
                    return 0;
                }
            }
        }

        public IEnumerable<ShopDetail> GetAllAsyncByProductID(object id)
        {
            try
            {
                _logger.LogInformation($"[GetAllAsyncByProductID] Start to connect to db with id: {id}");
                using var connection = Context.CreateConnection();

                var sqlQuery = $"SELECT ShopID, ProductID, Quantity FROM SHOPDETAIL " +
                    $"WHERE IsDeleted = 0 AND ProductID = @ProductID";

                var shopDetailList = connection.Query<ShopDetail>(sqlQuery, new { id });

                return shopDetailList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllAsyncByProductID] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ShopDetail> GetAllAsyncByShopID(object id)
        {
            try
            {
                _logger.LogInformation($"[GetAllAsyncByShopID] Start to connect to db with id: {id}");
                using var connection = Context.CreateConnection();

                var sqlQuery =
                    $"SELECT ProductID, Quantity FROM SHOPDETAIL WHERE ShopId = @ShopId; " +
                    $"SELECT ProductID, ProductName, Price FROM PRODUCT " +
                        $"WHERE ProductID IN" +
                        $"(SELECT ProductID FROM SHOPDETAIL WHERE ShopID = @ShopID)";

                using (var multi = connection.QueryMultiple(sqlQuery, new { ShopId = id }))
                {
                    var detailList = multi.Read<ShopDetail>().ToList();
                    var productList = multi.Read<Product>().ToList();

                    if (detailList.Count == 0 || productList.Count == 0)
                    {
                        throw new Exception("Not found detail list by id of product");
                    }

                    foreach (var item in detailList)
                    {
                        foreach (var product in productList)
                        {
                            if (product.ProductId == item.ProductId)
                            {
                                item.Product = product;
                                continue;
                            }
                        }
                    }

                    return detailList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllAsyncByShopID] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public int UpdateAsync(ShopDetail entity)
        {
            using (var connection = Context.CreateConnection())
            {
                _logger.LogInformation($"[UpdateAsyncShopDetail] Start to connect to db");
                var transaction = connection.BeginTransaction();

                var sqlQuery = $"UPDATE SHOPDETAIL SET Quantiy = @Quantity " +
                    $"WHERE ShopID = @ShopID AND ProductID = @ProductID AND IsDeleted = 0";

                try
                {
                    var recordChanges = connection.Execute(sqlQuery, entity);
                    transaction.Commit();

                    return recordChanges;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[UpdateAsyncShopDetail] Error: {ex.Message}");
                    transaction.Rollback();
                    return 0;
                }
            }
        }
    }
}
