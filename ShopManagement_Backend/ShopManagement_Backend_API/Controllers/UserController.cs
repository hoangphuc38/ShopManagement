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
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public UserController(
            IUserService userService,
            IMemoryCache cache,
            IConfiguration config)
        {
            _userService = userService;
            _cache = cache;
            _config = config;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.Parse(_config["CacheEntryOptions:AbsoluteExpiration"]),
                SlidingExpiration = TimeSpan.Parse(_config["CacheEntryOptions:SlidingExpiration"]),
            };
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            if (!_cache.TryGetValue("UserList", out BaseResponse? result))
            {
                result = _userService.GetAllUser();

                _cache.Set("UserList", result, _cacheEntryOptions);
            }

            return StatusCode(result.Status, result);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            if (!_cache.TryGetValue($"UserDetail_{id}", out BaseResponse? result))
            {
                result = _userService.GetUser(id);

                _cache.Set($"UserDetail_{id}", result, _cacheEntryOptions);
            }

            return StatusCode(result.Status, result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserRequest user)
        {
            var result = _userService.UpdateUser(id, user);

            _cache.Remove($"UserDetail_{id}");
            _cache.Remove("UserList");

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);

            _cache.Remove($"UserDetail_{id}");
            _cache.Remove("UserList");

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateUser(UserRequest user)
        {
            var result = _userService.CreateUser(user);

            _cache.Remove("UserList");

            return StatusCode(result.Status, result);
        }
    }
}
