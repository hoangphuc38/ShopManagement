using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public ShopController(
            IShopService shopService,
            IMemoryCache cache,
            IConfiguration config)
        {
            _shopService = shopService;
            _cache = cache;
            _config = config;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.Parse(_config["CacheEntryOptions:AbsoluteExpiration"]),
                SlidingExpiration = TimeSpan.Parse(_config["CacheEntryOptions:SlidingExpiration"]),
            };
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!_cache.TryGetValue("ShopList", out BaseResponse? result))
            {
                result = _shopService.GetAll();

                _cache.Set("ShopList", result, _cacheEntryOptions);
            }

            return StatusCode(result.Status, result);
        }

        [HttpGet("{userId}")]
        public IActionResult GetShopOfUser(int userId)
        {
            if (!_cache.TryGetValue("ShopDetail", out BaseResponse? result))
            {
                result = _shopService.GetShopOfUser(userId);

                _cache.Set("ShopDetail", result, _cacheEntryOptions);
            }

            return StatusCode(result.Status, result);
        }

        [HttpPut("{shopID}")]
        public IActionResult UpdateShop(int shopID, ShopRequest shop)
        {
            var result = _shopService.UpdateShop(shopID, shop);

            _cache.Remove("ShopDetail");
            _cache.Remove("ShopList");

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{shopID}")]
        public IActionResult DeleteShop(int shopID)
        {
            var result = _shopService.DeleteShop(shopID);

            _cache.Remove("ShopList");
            _cache.Remove("ShopDetail");

            return StatusCode(result.Status, result);
        }

        [HttpPost("{userID}")]
        public IActionResult CreateShop(int userID, ShopRequest shop)
        {
            var result = _shopService.CreateShop(userID, shop);

            _cache.Remove("ShopList");

            return StatusCode(result.Status, result);
        }
    }
}
