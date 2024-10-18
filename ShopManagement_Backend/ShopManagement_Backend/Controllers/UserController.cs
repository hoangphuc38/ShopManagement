using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;
using ShopManagement_Backend.Service;

namespace ShopManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-all")]
        public ActionResult GetAllUser()
        {
            var result = _userService.GetAllUser();

            return StatusCode(result.Status, result);
        }

        [HttpGet("get-by-id/{id}")]
        public ActionResult GetUser(int id)
        {
            var result = _userService.GetUser(id);

            return StatusCode(result.Status, result);
        }

        [HttpPut("update/{id}")]
        public ActionResult UpdateUser(int id, UserRequest user)
        {
            var result = _userService.UpdateUser(id, user);

            return StatusCode(result.Status, result);
        }

        [HttpPut("delete/{id}")]
        public ActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);

            return StatusCode(result.Status, result);
        }

        [HttpPost("new-user")]
        public ActionResult CreateUser(UserRequest user, string password)
        {
            var result = _userService.CreateUser(user, password);

            return StatusCode(result.Status, result);
        }
    }
}
