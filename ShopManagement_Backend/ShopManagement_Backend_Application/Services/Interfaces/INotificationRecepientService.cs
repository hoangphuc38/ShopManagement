using ShopManagement_Backend_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface INotificationRecepientService
    {
        Task<BaseResponse> GetAllNotification(int userID);
    }
}
