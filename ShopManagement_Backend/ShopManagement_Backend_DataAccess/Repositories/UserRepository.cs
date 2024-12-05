using Dapper;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(
            ShopManagementDbContext context,
            ILogger<UserRepository> logger) : base(context, logger) { }

        public async Task<User> GetUserByShopID(int shopID)
        {
            try
            {
                _logger.LogInformation($"[GetUserByShopID] Start to connect to db");
                using var connection = Context.GetDbConnection();

                var sqlQuery =
                    @"SELECT u.FullName
                      FROM USERS as u
                      INNER JOIN SHOP as s ON s.UserID = u.ID
                      WHERE u.IsDeleted = 0 AND s.ShopID = @ShopID";

                var parameters = new DynamicParameters();
                parameters.Add("@ShopID", shopID);

                var user = await connection.QueryFirstAsync<User>(sqlQuery, parameters);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetUserByShopID] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<User>? GetUsersWithPagination(
            int page, int pageSize, string sort, string filter, out int total)
        {
            try
            {
                _logger.LogInformation($"[GetUsersWithPagination] Start to connect to db");
                using var connection = Context.GetDbConnection();

                string sqlQuery =
                    $@"SELECT 
		                    ID, 
		                    UserName, 
		                    FullName,
                            Address,
                            SignUpDate,
		                    COUNT(ID) OVER() AS TotalRecords  
                    FROM Users
                    WHERE IsDeleted = 0 AND RoleID = 2
                    {filter}
                    {sort} OFFSET @skipRows ROWS FETCH NEXT @pageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("@skipRows", (page - 1) * pageSize);
                parameters.Add("@pageSize", pageSize);

                var userList = connection.Query<User>(sqlQuery, parameters);

                dynamic result = connection.QueryFirst(sqlQuery, parameters);

                total = result.TotalRecords;

                return userList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetUsersWithPagination] Error: {ex.Message}");
                total = 0;
                return new List<User>();
            }
        }
    }
}
