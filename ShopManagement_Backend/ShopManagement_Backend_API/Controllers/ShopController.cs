using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IMemoryCacheService _memoryCacheService;

        public ShopController(
            IShopService shopService,
            IMemoryCacheService memoryCacheService)
        {
            _shopService = shopService;
            _memoryCacheService = memoryCacheService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            BaseResponse result = new BaseResponse();
            if (!_memoryCacheService.CheckIfCacheExist("ShopList", result))
            {
                result = _shopService.GetAll();

                _memoryCacheService.SetCache("ShopList", result);
            }

            return StatusCode(result.Status, result);
        }

        [HttpGet("{userId}")]
        public IActionResult GetShopOfUser(int userId)
        {
            var result = _shopService.GetShopOfUser(userId);

            return StatusCode(result.Status, result);
        }

        [HttpPut("{shopID}")]
        public IActionResult UpdateShop(int shopID, ShopRequest shop)
        {
            var result = _shopService.UpdateShop(shopID, shop);

            _memoryCacheService.RemoveCache("ShopList");

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{shopID}")]
        public IActionResult DeleteShop(int shopID)
        {
            var result = _shopService.DeleteShop(shopID);

            _memoryCacheService.RemoveCache("ShopList");

            return StatusCode(result.Status, result);
        }

        [HttpPost("{userID}")]
        public IActionResult CreateShop(int userID, ShopRequest shop)
        {
            var result = _shopService.CreateShop(userID, shop);

            _memoryCacheService.RemoveCache("ShopList");

            return StatusCode(result.Status, result);
        }
    }
}
