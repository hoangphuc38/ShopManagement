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

        [HttpGet("get-all-of-shop/{shopID}")]
        public ActionResult GetAll(int shopID)
        {
            var result = _shopDetailService.GetAllOfShop(shopID);

            return StatusCode(result.Status, result);
        }

        [HttpPut("update-detail")]
        public ActionResult UpdateDetail(ShopDetailRequest request)
        {
            var result = _shopDetailService.UpdateDetail(request);

            return StatusCode(result.Status, result);
        }

        [HttpPut("delete")]
        public ActionResult DeleteProduct(int shopID, int productID)
        {
            var result = _shopDetailService.DeleteDetail(shopID, productID);

            return StatusCode(result.Status, result);
        }

        [HttpPost("new-detail")]
        public ActionResult CreateDetail(ShopDetailRequest detail)
        {
            var result = _shopDetailService.CreateDetail(detail);

            return StatusCode(result.Status, result);
        }
    }
}
