using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Services;

namespace ShopManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _productService.GetAll();

            return StatusCode(result.Status, result);
        }

        [HttpGet("{productID}")]
        public IActionResult GetDetailProduct(int productID)
        {
            var result = _productService.GetDetailProduct(productID);

            return StatusCode(result.Status, result);
        }

        [HttpPut("{productID}")]
        public IActionResult UpdateProduct(int productID, ProductRequest product)
        {
            var result = _productService.UpdateProduct(productID, product);

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{productID}")]
        public IActionResult DeleteProduct(int productID)
        {
            var result = _productService.DeleteProduct(productID);

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductRequest product)
        {
            var result = _productService.CreateProduct(product);

            return StatusCode(result.Status, result);
        }
    }
}
