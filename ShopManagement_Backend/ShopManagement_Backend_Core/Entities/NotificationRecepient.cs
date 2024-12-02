using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Core.Entities
{
    public class NotificationRecepient
    {
        public int NotificationRecepientId { get; set; }

        public int UserId { get; set; }

        public int NotificationId { get; set; }

        public bool? IsRead { get; set; }

        public virtual Notification Notification { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
