using Azure.Core;
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
        private readonly IMemoryCacheService _memoryCacheService;

        public ShopDetailController(
            IShopDetailService shopDetailService,
            IMemoryCacheService memoryCacheService)
        {
            _shopDetailService = shopDetailService;
            _memoryCacheService = memoryCacheService;
        }

        [HttpGet("{shopID}")]
        public IActionResult GetAll(int shopID)
        {
            BaseResponse result = new BaseResponse();
            if (!_memoryCacheService.CheckIfCacheExist($"Detail_{shopID}", result))
            {
                result = _shopDetailService.GetAllOfShop(shopID);

                _memoryCacheService.SetCache($"Detail_{shopID}", result);
            }

            return StatusCode(result.Status, result);
        }

        [HttpPut]
        public IActionResult UpdateDetail(ShopDetailRequest request)
        {           
            var result = _shopDetailService.UpdateDetail(request);

            _memoryCacheService.RemoveCache($"Detail_{request.ShopId}");

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{shopID}/{productID}")]
        public IActionResult DeleteProduct(int shopID, int productID)
        {
            var result = _shopDetailService.DeleteDetail(shopID, productID);

            _memoryCacheService.RemoveCache($"Detail_{shopID}");

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateDetail(ShopDetailRequest detail)
        {
            var result = _shopDetailService.CreateDetail(detail);

            _memoryCacheService.RemoveCache($"Detail_{detail.ShopId}");

            return StatusCode(result.Status, result);
        }
    }
}
