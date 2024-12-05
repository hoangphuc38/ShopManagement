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

        public IEnumerable<ShopDetail> GetShopDetailWithPagination(
            object id,
            int page, int pageSize, string sort, string filter, out int total)
        {
            try
            {
                _logger.LogInformation($"[GetAllAsyncByShopID] Start to connect to db with id: {id}");
                using var connection = Context.GetDbConnection();

                var sqlQuery =
                    $@"SELECT 
                        s.ProductID, 
                        s.Quantity, 
                        p.ProductName, 
                        p.Price,
                        COUNT(s.ProductID) OVER() as TotalRecords
                      FROM SHOPDETAIL as s
                      INNER JOIN PRODUCT as p ON s.ProductID = p.ProductID 
                      WHERE s.ShopID = @ShopID AND s.IsDeleted = 0 {filter}
                      {sort}
                      OFFSET @skipRows ROWS 
                      FETCH NEXT @pageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("@ShopID", id);
                parameters.Add("@skipRows", (page - 1) * pageSize);
                parameters.Add("@pageSize", pageSize);

                var detailList = connection.Query<ShopDetail>(sqlQuery, parameters);

                var productList = connection.Query<Product>(sqlQuery, parameters);

                if (detailList.Count() == 0 || productList.Count() == 0)
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

                dynamic result = connection.QueryFirst(sqlQuery, parameters);

                total = result.TotalRecords;

                return detailList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllAsyncByShopID] Error: {ex.Message}");
                total = 0;
                return new List<ShopDetail>();
            }
        }
    }
}
