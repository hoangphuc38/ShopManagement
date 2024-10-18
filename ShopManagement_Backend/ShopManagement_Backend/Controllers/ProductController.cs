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

        [HttpGet("get-all")]
        public ActionResult GetAll()
        {
            var result = _productService.GetAll();

            return StatusCode(result.Status, result);
        }

        [HttpGet("get-detail-product/{productID}")]
        public ActionResult GetDetailProduct(int productID)
        {
            var result = _productService.GetDetailProduct(productID);

            return StatusCode(result.Status, result);
        }

        [HttpPut("update/{productID}")]
        public ActionResult UpdateProduct(int productID, ProductRequest product)
        {
            var result = _productService.UpdateProduct(productID, product);

            return StatusCode(result.Status, result);
        }

        [HttpPut("delete/{productID}")]
        public ActionResult DeleteProduct(int productID)
        {
            var result = _productService.DeleteProduct(productID);

            return StatusCode(result.Status, result);
        }

        [HttpPost("new-product")]
        public ActionResult CreateProduct(ProductRequest product)
        {
            var result = _productService.CreateProduct(product);

            return StatusCode(result.Status, result);
        }
    }
}
