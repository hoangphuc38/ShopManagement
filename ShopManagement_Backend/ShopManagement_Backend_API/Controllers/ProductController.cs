using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(
            IProductService productService)
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
