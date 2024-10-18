using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("get-all")]
        public ActionResult GetAll()
        {
            var result = _shopService.GetAll();

            return StatusCode(result.Status, result);
        }

        [HttpGet("get-shops-of-user/{id}")]
        public ActionResult GetShopOfUser(int id)
        {
            var result = _shopService.GetShopOfUser(id);

            return StatusCode(result.Status, result);
        }
    }
}
