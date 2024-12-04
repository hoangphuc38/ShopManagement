using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/shop-detail")]
    [ApiController]
    [CustomAttributes.Authorize]
    public class ShopDetailController : ControllerBase
    {
        private readonly IShopDetailService _shopDetailService;

        public ShopDetailController(
            IShopDetailService shopDetailService)
        {
            _shopDetailService = shopDetailService;
        }

        [HttpGet("{shopID}")]
        public async Task<IActionResult> GetShopDetailWithPagination(int shopID, [FromQuery] ShopDetailPaginationRequest request)
        {
            var result = await _shopDetailService.GetShopDetailWithPagination(shopID, request);

            return StatusCode(result.Status, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDetail(ShopDetailRequest request)
        {           
            var result = await _shopDetailService.UpdateDetail(request);

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{shopID}/{productID}")]
        public async Task<IActionResult> DeleteProduct(int shopID, int productID)
        {
            var result = await _shopDetailService.DeleteDetail(shopID, productID);

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDetail(ShopDetailRequest detail)
        {
            var result = await _shopDetailService.CreateDetail(detail);

            return StatusCode(result.Status, result);
        }
    }
}
