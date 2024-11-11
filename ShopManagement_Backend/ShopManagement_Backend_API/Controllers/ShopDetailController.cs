using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopDetailController : ControllerBase
    {
        private readonly IShopDetailService _shopDetailService;

        public ShopDetailController(
            IShopDetailService shopDetailService)
        {
            _shopDetailService = shopDetailService;
        }

        [HttpGet("{shopID}")]
        public IActionResult GetAll(int shopID)
        {
            var result = _shopDetailService.GetAllOfShop(shopID);

            return StatusCode(result.Status, result);
        }

        [HttpPut]
        public IActionResult UpdateDetail(ShopDetailRequest request)
        {           
            var result = _shopDetailService.UpdateDetail(request);

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{shopID}/{productID}")]
        public IActionResult DeleteProduct(int shopID, int productID)
        {
            var result = _shopDetailService.DeleteDetail(shopID, productID);

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateDetail(ShopDetailRequest detail)
        {
            var result = _shopDetailService.CreateDetail(detail);

            return StatusCode(result.Status, result);
        }
    }
}
