using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopDetailController : ControllerBase
    {
        private IShopDetailService _shopDetailService;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public ShopDetailController(
            IShopDetailService shopDetailService,
            IMemoryCache cache,
            IConfiguration config)
        {
            _shopDetailService = shopDetailService;
            _cache = cache;
            _config = config;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.Parse(_config["CacheEntryOptions:AbsoluteExpiration"]),
                SlidingExpiration = TimeSpan.Parse(_config["CacheEntryOptions:SlidingExpiration"]),
            };
        }

        [HttpGet("{shopID}")]
        public IActionResult GetAll(int shopID)
        {
            if (!_cache.TryGetValue("Detail", out BaseResponse? result))
            {
                result = _shopDetailService.GetAllOfShop(shopID);

                _cache.Set("Detail", result, _cacheEntryOptions);
            }

            return StatusCode(result.Status, result);
        }

        [HttpPut]
        public IActionResult UpdateDetail(ShopDetailRequest request)
        {           
            var result = _shopDetailService.UpdateDetail(request);

            _cache.Remove("Detail");

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{shopID}/{productID}")]
        public IActionResult DeleteProduct(int shopID, int productID)
        {
            var result = _shopDetailService.DeleteDetail(shopID, productID);

            _cache.Remove("Detail");

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateDetail(ShopDetailRequest detail)
        {
            var result = _shopDetailService.CreateDetail(detail);

            _cache.Remove("Detail");

            return StatusCode(result.Status, result);
        }
    }
}
