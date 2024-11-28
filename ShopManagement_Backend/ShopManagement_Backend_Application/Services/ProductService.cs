using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepo;
        private readonly IShopDetailRepository _shopDetailRepo;
        private readonly ILogger<ProductService> _logger;
        private readonly IMemoryCacheService _memoryCacheService;

        public ProductService(
            IMapper mapper,
            IProductRepository productRepo,
            IShopDetailRepository shopDetailRepo,
            ILogger<ProductService> logger,
            IMemoryCacheService memoryCacheService)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _shopDetailRepo = shopDetailRepo;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<BaseResponse> GetAll()
        {
            try
            {
                _logger.LogInformation($"[GetAllProduct] Start to get all products.");

                var response = _memoryCacheService.GetCacheData("ProductList");

                if (response == null)
                {
                    var productList = await _productRepo.GetAllProducts();

                    var productMapperList = _mapper.Map<List<ProductResponse>>(productList);

                    if (productMapperList == null)
                    {
                        return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get all");
                    }

                    response = new BaseResponse(productMapperList);

                    _memoryCacheService.SetCache("ProductList", response);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get all");
            }
            
        }

        public async Task<BaseResponse> GetDetailProduct(int id)
        {
            try
            {
                _logger.LogInformation($"[GetDetailProduct] Start to get detail product by ProductId: {id}.");

                var response = _memoryCacheService.GetCacheData($"Product_{id}");

                if (response == null)
                {
                    var product = await _productRepo.GetFirstAsync(t => t.ProductId == id && !t.IsDeleted);

                    if (product == null)
                    {
                        return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                    }

                    var productMapper = _mapper.Map<ProductResponse>(product);

                    response = new BaseResponse(productMapper);

                    _memoryCacheService.SetCache($"Product_{id}", response);
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetDetailProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get detail");
            }
        }

        public async Task<BaseResponse> UpdateProduct(int id, ProductRequest request)
        {
            try
            {
                _logger.LogInformation($"[UpdateProduct] Start to update product with ProductId: {id}, " +
                    $"ProductName: {request.ProductName}, Price: {request.Price}");

                var product = await _productRepo.GetFirstAsync(t => t.ProductId == id && !t.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                product.ProductName = request.ProductName;
                product.Price = request.Price;

                await _productRepo.UpdateAsync(product);

                _memoryCacheService.RemoveCache($"Product_{id}");
                _memoryCacheService.RemoveCache("ProductList");

                return new BaseResponse("Update product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update");
            }
            
        }

        public async Task<BaseResponse> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation($"[DeleteProduct] Start to delete product by ProductId: {id}.");
                var product = await _productRepo.GetFirstAsync(t => t.ProductId == id && !t.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                product.IsDeleted = true;
                await _productRepo.UpdateAsync(product);


                var detailList = await _shopDetailRepo.GetAllAsync(c => c.ProductId == id && !c.IsDeleted);
                foreach (var detail in detailList)
                {
                    detail.IsDeleted = true;
                    await _shopDetailRepo.UpdateAsync(detail);
                }

                _memoryCacheService.RemoveCache($"Product_{id}");
                _memoryCacheService.RemoveCache("ProductList");

                return new BaseResponse("Delete product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete");
            }
            
        }

        public async Task<BaseResponse> CreateProduct(ProductRequest request)
        {
            try
            {
                _logger.LogInformation($"[CreateProduct] Start to create product. " +
                    $"ProductName: {request.ProductName}, Price: {request.Price}");
                var product = _mapper.Map<Product>(request);
                product.IsDeleted = false;

                await _productRepo.AddAsync(product);

                _memoryCacheService.RemoveCache("ProductList");

                return new BaseResponse("Create product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CreateProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create");
            }          
        }
    }
}
