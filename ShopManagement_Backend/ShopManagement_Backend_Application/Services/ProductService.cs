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

        public ProductService(
            IMapper mapper,
            IProductRepository productRepo,
            IShopDetailRepository shopDetailRepo,
            ILogger<ProductService> logger)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _shopDetailRepo = shopDetailRepo;
            _logger = logger;
        }

        public BaseResponse GetAll()
        {
            try
            {
                _logger.LogInformation($"[GetAllProduct] Start to get all products.");
                var productList = _productRepo.GetAllAsync(t => !t.IsDeleted);
                var responseList = _mapper.Map<List<ProductResponse>>(productList);

                return new BaseResponse(responseList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get all");
            }
            
        }

        public BaseResponse GetDetailProduct(int id)
        {
            try
            {
                _logger.LogInformation($"[GetDetailProduct] Start to get detail product by ProductId: {id}.");
                var product = _productRepo.GetFirstAsync(t => t.ProductId == id && !t.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                var response = _mapper.Map<ProductResponse>(product);

                return new BaseResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetDetailProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get detail");
            }
        }

        public BaseResponse UpdateProduct(int id, ProductRequest request)
        {
            try
            {
                _logger.LogInformation($"[UpdateProduct] Start to update product with ProductId: {id}, " +
                    $"ProductName: {request.ProductName}, Price: {request.Price}");
                var product = _productRepo.GetFirstAsync(t => t.ProductId == id && !t.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                product.ProductName = request.ProductName;
                product.Price = request.Price;

                _productRepo.UpdateAsync(product);

                return new BaseResponse("Update product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update");
            }
            
        }

        public BaseResponse DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation($"[DeleteProduct] Start to delete product by ProductId: {id}.");
                var product = _productRepo.GetFirstAsync(t => t.ProductId == id && !t.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                product.IsDeleted = true;
                _productRepo.UpdateAsync(product);


                var detailList = _shopDetailRepo.GetAllAsync(c => c.ProductId == id && !c.IsDeleted);
                foreach (var detail in detailList)
                {
                    detail.IsDeleted = true;
                    _shopDetailRepo.UpdateAsync(detail);
                }

                return new BaseResponse("Delete product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete");
            }
            
        }

        public BaseResponse CreateProduct(ProductRequest request)
        {
            try
            {
                _logger.LogInformation($"[CreateProduct] Start to create product. " +
                    $"ProductName: {request.ProductName}, Price: {request.Price}");
                var product = _mapper.Map<Product>(request);
                product.IsDeleted = false;

                _productRepo.AddAsync(product);

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
