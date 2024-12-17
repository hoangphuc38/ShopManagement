using Dapper;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class NotificationRecepientRepository : BaseRepository<NotificationRecepient>, INotificationRecepientRepository
    {
        public NotificationRecepientRepository(
            ShopManagementDbContext context,
            ILogger<NotificationRecepientRepository> logger) : base(context, logger) { }

        public IEnumerable<NotificationRecepient> GetAllNotifications(int userID)
        {
            try
            {
                _logger.LogInformation($"[GetAllNotifications] Start to connect to db");

                using var connection = Context.GetDbConnection();

                var sqlQuery =
                    $@"SELECT 
                        s.Title, 
                        s.Content, 
                        s.SentDate, 
                        p.NotificationRecepientID,
                        p.UserID,
                        p.NotificationID,
                        p.IsRead
                      FROM NotificationRecepient as p
                      INNER JOIN Notification as s ON p.NotificationID = s.NotificationID 
                      WHERE p.UserID = @UserID
                      ORDER BY s.SentDate DESC";

                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);

                var recepList = connection.Query<NotificationRecepient>(sqlQuery, parameters);

                var notificationList = connection.Query<Notification>(sqlQuery, parameters);

                if (recepList.Count() == 0 || notificationList.Count() == 0)
                {
                    throw new Exception("Not found notification list by id of user");
                }

                foreach (var item in recepList)
                {
                    foreach (var notification in notificationList)
                    {
                        if (item.NotificationId == notification.NotificationId)
                        {
                            item.Notification = notification;
                            continue;
                        }
                    }
                }

                return recepList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllNotifications] Error: {ex.Message}");
                return new List<NotificationRecepient>();
            }
        }

        public int GetUnreadNotification(int userID)
        {
            try
            {
                _logger.LogInformation($"[GetUnreadNotification] Start to connect to db");

                using var connection = Context.GetDbConnection();

                var sqlQuery =
                    $@"SELECT
                        COUNT(p.IsRead)
                      FROM NotificationRecepient as p
                      INNER JOIN Notification as s ON p.NotificationID = s.NotificationID 
                      WHERE p.UserID = @UserID AND p.IsRead = 0";

                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);

                var recepList = connection.QueryFirst<int>(sqlQuery, parameters);

                return recepList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetUnreadNotification] Error: {ex.Message}");
                return 0;
            }
        }
    }
}
