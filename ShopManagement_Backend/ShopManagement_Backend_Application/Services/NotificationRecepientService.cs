using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Notification;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Reflection;
using System.Resources;

namespace ShopManagement_Backend_Application.Services
{
    public class NotificationRecepientService : INotificationRecepientService
    {
        private readonly IMapper _mapper;
        private readonly INotificationRecepientRepository _notiRecepRepo;
        private readonly INotificationRepository _notiRepo;
        private readonly ILogger<NotificationRecepientService> _logger;
        private readonly ResourceManager _resource;

        public NotificationRecepientService(
            IMapper mapper,
            INotificationRecepientRepository notiRecepRepo, 
            INotificationRepository notiRepo, 
            ILogger<NotificationRecepientService> logger)
        {
            _mapper = mapper;
            _notiRecepRepo = notiRecepRepo;
            _notiRepo = notiRepo;
            _logger = logger;
            _resource = new ResourceManager(
                "ShopManagement_Backend_Application.Resources.Messages.NotificationMessages",
                Assembly.GetExecutingAssembly());
        }

        public async Task<BaseResponse> GetAllNotification(int userID)
        {
            try
            {
                _logger.LogInformation($"[GetAllNotification] Start to get notification of user with id {userID}");

                var notificationList = _notiRecepRepo.GetAllNotifications(userID);

                int total = _notiRecepRepo.GetUnreadNotification(userID);

                var notificationMapper = _mapper.Map<List<NotificationResponse>>(notificationList);

                var response = new TotalNotificationResponse
                {
                    UnreadNotification = total,
                    Results = notificationMapper
                };

                return new BaseResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllNotification] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetNotificationFailed") ?? "");
            }
        }

        public async Task<BaseResponse> ReadNotification(int notificationID)
        {
            try
            {
                _logger.LogInformation($"[ReadNotification] Start to get notification with id {notificationID}");

                var notification = await _notiRecepRepo.GetFirstAsync(c => c.NotificationRecepientId == notificationID);

                if (notification == null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "Not found notification");
                }

                notification.IsRead = true;

                await _notiRecepRepo.UpdateAsync(notification);

                return new BaseResponse("Mark read notification");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ReadNotification] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetNotificationFailed") ?? "");
            }
        }
    }
}
