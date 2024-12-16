using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_API.CustomAttributes;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/notification")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRecepientService _notificationService;

        public NotificationController(INotificationRecepientService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetAllNotifications(int userID)
        {
            var result = await _notificationService.GetAllNotification(userID);

            return StatusCode(result.Status, result);
        }
    }
}
