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
    }
}
