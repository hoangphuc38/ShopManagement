using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(
            IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _shopService.GetAll();

            return StatusCode(result.Status, result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetShopOfUser(int userId)
        {
            var result = await _shopService.GetShopOfUser(userId);

            return StatusCode(result.Status, result);
        }

        [HttpPut("{shopID}")]
        public async Task<IActionResult> UpdateShop(int shopID, ShopRequest shop)
        {
            var result = await _shopService.UpdateShop(shopID, shop);

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{shopID}")]
        public async Task<IActionResult> DeleteShop(int shopID)
        {
            var result = await _shopService.DeleteShop(shopID);

            return StatusCode(result.Status, result);
        }

        [HttpPost("{userID}")]
        public async Task<IActionResult> CreateShop(int userID, ShopRequest shop)
        {
            var result = await _shopService.CreateShop(userID, shop);

            return StatusCode(result.Status, result);
        }
    }
}
