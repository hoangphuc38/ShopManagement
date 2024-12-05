using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_API.CustomAttributes;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAttributes.Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(
            IProductService productService)
        {
            _productService = productService;
        }        

        [HttpGet]
        public async Task<IActionResult> GetProductsWithPagination([FromQuery] ProductPaginationRequest request)
        {
            var result = await _productService.GetProductsWithPagination(request);
            
            return StatusCode(result.Status, result);
        }

        [HttpGet("{productID}")]
        public async Task<IActionResult> GetDetailProduct(int productID)
        {       
            var result = await _productService.GetDetailProduct(productID);

            return StatusCode(result.Status, result);
        }

        [HttpPut("{productID}")]
        [Role(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int productID, ProductRequest product)
        {
            var result = await _productService.UpdateProduct(productID, product);

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{productID}")]
        [Role(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int productID)
        {
            var result = await _productService.DeleteProduct(productID);

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        [Role(Roles = "admin")]
        public async Task<IActionResult> CreateProduct(ProductRequest product)
        {
            var result = await _productService.CreateProduct(product);

            return StatusCode(result.Status, result);
        }
    }
}
