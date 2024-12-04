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
    }
}
