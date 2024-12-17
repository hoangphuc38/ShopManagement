using ShopManagement_Backend_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface INotificationRecepientRepository : IBaseRepository<NotificationRecepient>
    {
        IEnumerable<NotificationRecepient> GetAllNotifications(int userID);

        int GetUnreadNotification(int userID);
    }
}
