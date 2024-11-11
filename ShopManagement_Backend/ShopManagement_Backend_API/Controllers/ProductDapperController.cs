using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_Application.DapperServices.Interfaces;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDapperController : ControllerBase
    {
        private readonly IProductDapService _productService;
        private readonly IMemoryCacheService _memoryCacheService;

        public ProductDapperController(
            IProductDapService productService,
            IMemoryCacheService memoryCacheService)
        {
            _productService = productService;
            _memoryCacheService = memoryCacheService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            BaseResponse result = new BaseResponse();
            if (!_memoryCacheService.CheckIfCacheExist("ProductList_Dapper", result))
            {
                result = _productService.GetAll();

                _memoryCacheService.SetCache("ProductList_Dapper", result);
            }

            return StatusCode(result.Status, result);
        }

        [HttpGet("{productID}")]
        public IActionResult GetDetailProduct(int productID)
        {
            BaseResponse result = new BaseResponse();
            if (!_memoryCacheService.CheckIfCacheExist($"ProductDetail_{productID}", result))
            {
                result = _productService.GetDetailProduct(productID);

                _memoryCacheService.SetCache($"ProductDetail_{productID}", result);
            }

            return StatusCode(result.Status, result);
        }

        [HttpPut("{productID}")]
        public IActionResult UpdateProduct(int productID, ProductRequest product)
        {
            var result = _productService.UpdateProduct(productID, product);

            _memoryCacheService.RemoveCache($"ProductDetail_{productID}");
            _memoryCacheService.RemoveCache("ProductList_Dapper");

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductRequest product)
        {
            var result = _productService.CreateProduct(product);

            _memoryCacheService.RemoveCache("ProductList_Dapper");

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{productID}")]
        public IActionResult DeleteProduct(int productID)
        {
            var result = _productService.DeleteProduct(productID);

            _memoryCacheService.RemoveCache($"ProductDetail_{productID}");
            _memoryCacheService.RemoveCache("ProductList_Dapper");

            return StatusCode(result.Status, result);
        }
    }
}
