using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Core.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? SentDate { get; set; }

        public string? SenderInfo { get; set; }

        public virtual ICollection<NotificationRecepient> NotificationRecepients { get; set; } = new List<NotificationRecepient>();
    }
}
