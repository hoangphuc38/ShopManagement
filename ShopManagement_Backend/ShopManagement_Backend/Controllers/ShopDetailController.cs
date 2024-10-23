using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Services;

namespace ShopManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopDetailController : ControllerBase
    {
        private ShopDetailService _shopDetailService;

        public ShopDetailController(ShopDetailService shopDetailService)
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
