using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_API.CustomAttributes;
using ShopManagement_Backend_Application.Models.Asset;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/assets")]
    [ApiController]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IImageService _imageService;

        public AssetsController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadAsync([FromForm] AssetRequest request)
        {
            var result = await _imageService.UploadAsync(request);

            return StatusCode(result.Status, result);
        }

        [HttpPost("delete-image")]
        public async Task<IActionResult> DeleteAsync(AssetDeleteRequest request)
        {
            var result = await _imageService.DeleteAsync(request);

            return StatusCode(result.Status, result);
        }
    }
}
