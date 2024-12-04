using Dapper;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class ShopRepository : BaseRepository<Shop>, IShopRepository
    {
        public ShopRepository(
            ShopManagementDbContext context,
            ILogger<ShopRepository> logger) : base(context, logger) { }

        public IEnumerable<Shop>? GetShopsWithPagination(
            int page, int pageSize, string sort, string filter, out int total)
        {
            try
            {
                _logger.LogInformation($"[GetShopsWithPagination] Start to connect to db");

                using var connection = Context.GetDbConnection();

                string sqlQuery =
                    $@"SELECT 
                        s.ShopID, 
                        s.ShopName, 
                        s.ShopAddress, 
                        s.CreatedDate,
                        COUNT(ShopID) OVER() AS TotalRecords 
                      FROM SHOP as s
                      INNER JOIN USERS as u ON s.UserID = u.ID
                      WHERE s.IsDeleted = 0 {filter} 
                      {sort}
                      OFFSET @skipRows ROWS 
                      FETCH NEXT @pageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("@skipRows", (page - 1) * pageSize);
                parameters.Add("@pageSize", pageSize);

                var shopList = connection.Query<Shop>(sqlQuery, parameters);

                dynamic result = connection.QueryFirst(sqlQuery, parameters);

                total = result.TotalRecords;

                return shopList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetShopsWithPagination] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Shop> GetShopByUserID(
            object userID, 
            int page, int pageSize, string sort, string filter, out int total)
        {
            try
            {
                _logger.LogInformation($"[GetShopByUserID] Start to connect to db");
                using var connection = Context.GetDbConnection();

                var sqlQuery =
                    $@"SELECT 
                        s.ShopID, 
                        s.ShopName, 
                        s.ShopAddress, 
                        s.CreatedDate,
                        COUNT(ShopID) OVER() AS TotalRecords 
                      FROM SHOP as s
                      INNER JOIN USERS as u ON s.UserID = u.ID
                      WHERE s.IsDeleted = 0 AND s.UserID = @UserID {filter}
                      {sort}
                      OFFSET @skipRows ROWS 
                      FETCH NEXT @pageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);
                parameters.Add("@skipRows", (page - 1) * pageSize);
                parameters.Add("@pageSize", pageSize);

                var shopList = connection.Query<Shop>(sqlQuery, parameters);

                dynamic result = connection.QueryFirst(sqlQuery, parameters);

                total = result.TotalRecords;

                return shopList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetShopByUserID] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
