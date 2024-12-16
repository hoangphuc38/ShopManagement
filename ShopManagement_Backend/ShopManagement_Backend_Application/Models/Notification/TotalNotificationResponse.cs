using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Models.Notification
{
    class TotalNotificationResponse
    {
        public int UnreadNotification { get; set; }

        public List<NotificationResponse> Results { get; set; }
    }
}
