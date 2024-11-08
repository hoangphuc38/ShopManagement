using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCacheService _memoryCacheService;

        public UserController(
            IUserService userService,
            IMemoryCacheService memoryCacheService)
        {
            _userService = userService;
            _memoryCacheService = memoryCacheService;
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            BaseResponse result = new BaseResponse();
            if (!_memoryCacheService.CheckIfCacheExist("UserList", result))
            {
                result = _userService.GetAllUser();

                _memoryCacheService.SetCache("UserList", result);
            }

            return StatusCode(result.Status, result);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            BaseResponse result = new BaseResponse();
            if (!_memoryCacheService.CheckIfCacheExist($"UserDetail_{id}", result))
            {
                result = _userService.GetUser(id);

                _memoryCacheService.SetCache($"UserDetail_{id}", result);
            }

            return StatusCode(result.Status, result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserRequest user)
        {
            var result = _userService.UpdateUser(id, user);

            _memoryCacheService.RemoveCache($"UserDetail_{id}");
            _memoryCacheService.RemoveCache("UserList");

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);

            _memoryCacheService.RemoveCache($"UserDetail_{id}");
            _memoryCacheService.RemoveCache("UserList");

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateUser(UserRequest user)
        {
            var result = _userService.CreateUser(user);

            _memoryCacheService.RemoveCache("UserList");

            return StatusCode(result.Status, result);
        }
    }
}
