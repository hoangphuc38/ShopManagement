using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_API.CustomAttributes;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [CustomAttributes.Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Role(Roles = "admin")]
        public async Task<IActionResult> GetUsersWithPagination([FromQuery] UserPaginationRequest request)
        {
            var result = await _userService.GetUsersWithPagination(request);

            return StatusCode(result.Status, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUser(id);

            return StatusCode(result.Status, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserRequest user)
        {
            var result = await _userService.UpdateUser(id, user);

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{id}")]
        [Role(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);

            return StatusCode(result.Status, result);
        }
    }
}
