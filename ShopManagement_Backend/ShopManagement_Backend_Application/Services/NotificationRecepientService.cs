using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Notification;
using ShopManagement_Backend_Application.Services.Interfaces;
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

                var notificationList = await _notiRecepRepo.GetAllAsync(c => c.UserId == userID);

                var unread = await _notiRecepRepo.GetAllAsync(c => c.UserId == userID && c.IsRead == false);

                int total = unread.Count();

                foreach (var notification in notificationList)
                {
                    var content = await _notiRepo.GetFirstAsync(c => c.NotificationId == notification.NotificationId);

                    if (content == null)
                    {
                        return new BaseResponse(StatusCodes.Status500InternalServerError, "Content null");
                    }
                }

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
    }
}
