using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            var result = _userService.GetAllUser();

            return StatusCode(result.Status, result);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var result = _userService.GetUser(id);

            return StatusCode(result.Status, result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserRequest user)
        {
            var result = _userService.UpdateUser(id, user);

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateUser(UserRequest user)
        {
            var result = _userService.CreateUser(user);

            return StatusCode(result.Status, result);
        }
    }
}
