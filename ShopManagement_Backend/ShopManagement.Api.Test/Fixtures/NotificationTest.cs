using Azure.Core;
using ShopManagement_Backend_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Api.Test.Fixtures
{
    public class NotificationTest
    {
        public Notification notification = new Notification
        {
            Title = "Sản phẩm mới",
            Content = $"Đã thêm sản phẩm mới: Argentina với giá 100",
            SentDate = DateTime.Now,
            SenderInfo = "Admin"
        };
    }
}
