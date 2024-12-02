using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Models.Notification
{
    public class NotificationResponse
    {
        public int NotificationRecepientID { get; set; }

        public NotificationContent? NotificationContent { get; set; }

        public bool IsRead { get; set; }
    }

    public class NotificationContent
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? SenderInfo { get; set; }
    }
}
