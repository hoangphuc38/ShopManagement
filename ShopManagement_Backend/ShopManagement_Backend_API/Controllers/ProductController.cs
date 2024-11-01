using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public ProductController(
            IProductService productService,
            IMemoryCache cache,
            IConfiguration config)
        {
            _productService = productService;
            _cache = cache;
            _config = config;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.Parse(_config["CacheEntryOptions:AbsoluteExpiration"]),
                SlidingExpiration = TimeSpan.Parse(_config["CacheEntryOptions:SlidingExpiration"]),
            };
        }        

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!_cache.TryGetValue("ProductList", out BaseResponse? result))
            {
                result = _productService.GetAll();

                _cache.Set("ProductList", result, _cacheEntryOptions);
            }
            
            return StatusCode(result.Status, result);
        }

        [HttpGet("{productID}")]
        public IActionResult GetDetailProduct(int productID)
        {
            if (!_cache.TryGetValue("ProductDetail", out BaseResponse? result))
            {
                result = _productService.GetDetailProduct(productID);

                _cache.Set("ProductDetail", result, _cacheEntryOptions);
            }

            return StatusCode(result.Status, result);
        }

        [HttpPut("{productID}")]
        public IActionResult UpdateProduct(int productID, ProductRequest product)
        {
            var result = _productService.UpdateProduct(productID, product);

            _cache.Remove("ProductDetail");
            _cache.Remove("ProductList");

            return StatusCode(result.Status, result);
        }

        [HttpDelete("{productID}")]
        public IActionResult DeleteProduct(int productID)
        {
            var result = _productService.DeleteProduct(productID);

            _cache.Remove("ProductDetail");
            _cache.Remove("ProductList");

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductRequest product)
        {
            var result = _productService.CreateProduct(product);

            _cache.Remove("ProductList");

            return StatusCode(result.Status, result);
        }
    }
}
