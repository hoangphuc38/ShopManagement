using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;
using ShopManagement_Backend.Services;

namespace ShopManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ShopService _shopService;

        public ShopController(ShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _shopService.GetAll();

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

            return StatusCode(result.Status, result);
        }

        [HttpPut("delete/{shopID}")]
        public IActionResult DeleteShop(int shopID)
        {
            var result = _shopService.DeleteShop(shopID);

            return StatusCode(result.Status, result);
        }

        [HttpPost("{userID}")]
        public IActionResult CreateShop(int userID, ShopRequest shop)
        {
            var result = _shopService.CreateShop(userID, shop);

            return StatusCode(result.Status, result);
        }
    }
}
