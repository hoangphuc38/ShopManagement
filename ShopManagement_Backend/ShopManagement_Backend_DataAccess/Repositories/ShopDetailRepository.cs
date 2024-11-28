using Dapper;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class ShopDetailRepository : BaseRepository<ShopDetail>, IShopDetailRepository
    {
        public ShopDetailRepository(
            ShopManagementDbContext context,
            ILogger<ShopDetailRepository> logger) : base(context, logger) { }

        public async Task<IEnumerable<ShopDetail>> GetAllAsyncByShopID(object id)
        {
            try
            {
                _logger.LogInformation($"[GetAllAsyncByShopID] Start to connect to db with id: {id}");
                using var connection = Context.GetDbConnection();

                var sqlQuery =
                    $"SELECT ProductID, Quantity FROM SHOPDETAIL WHERE ShopId = @ShopId AND IsDeleted = 0; " +
                    $"SELECT ProductID, ProductName, Price FROM PRODUCT " +
                        $"WHERE ProductID IN" +
                        $"(SELECT ProductID FROM SHOPDETAIL WHERE ShopID = @ShopID)";

                var parameters = new DynamicParameters();
                parameters.Add("@ShopID", id);

                using (var multi = await connection.QueryMultipleAsync(sqlQuery, parameters))
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
    }
}
