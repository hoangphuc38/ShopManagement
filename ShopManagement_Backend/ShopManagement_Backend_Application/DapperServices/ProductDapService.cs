using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.DapperServices.Interfaces;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces;

namespace ShopManagement_Backend_Application.DapperServices
{
    public class ProductDapService : IProductDapService
    {
        private readonly IMapper _mapper;
        private readonly IProductDapRepository _productRepo;
        private readonly IShopDetailDapRepository _shopDetailDapRepo;
        private readonly ILogger<ProductDapService> _logger;    
        
        public ProductDapService(
            IMapper mapper,
            IProductDapRepository productRepo,
            IShopDetailDapRepository shopDetailDapRepo,
            ILogger<ProductDapService> logger)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _shopDetailDapRepo = shopDetailDapRepo;
            _logger = logger;
        }

        public BaseResponse CreateProduct(ProductRequest request)
        {
            try
            {
                _logger.LogInformation(
                    $"[CreateProduct] Start to create product. ProductName: {request.ProductName}, " +
                    $"Price: {request.Price}");

                var product = _mapper.Map<Product>(request);
                product.IsDeleted = false;

                var recordChanges = _productRepo.AddAsync(product);

                if (recordChanges == 0)
                {
                    return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create");
                }

                return new BaseResponse("Create product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CreateProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create product");
            }
            
        }

        public BaseResponse DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation($"[DeleteProduct] Start to delete product with id: {id}");
                var product = _productRepo.GetFirstOrNullAsync(id);

                if (product == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                //Delete product
                var recordChangesProduct = _productRepo.DeleteAsync(product);

                if (recordChangesProduct == 0)
                {
                    return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete");
                }

                //Delete product in shop detail
                var shopDetailList = _shopDetailDapRepo.GetAllAsyncByProductID(id);

                foreach (var shopDetail in shopDetailList)
                {
                    var recordChangesShopDetail = _shopDetailDapRepo.DeleteAsync(shopDetail);

                    if (recordChangesShopDetail == 0)
                    {
                        return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete");
                    }
                }

                return new BaseResponse("Delte product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete product");
            }
        }

        public BaseResponse GetAll()
        {
            try
            {
                _logger.LogInformation($"[GetAllProduct] Start to get all products");

                var productList = _productRepo.GetAllAsync();

                var responseList = _mapper.Map<List<ProductResponse>>(productList);

                return new BaseResponse(responseList);
            }
            catch (Exception ex )
            {
                _logger.LogError($"[GetAllProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get all product");
            }
        }

        public BaseResponse GetDetailProduct(int id)
        {
            try
            {
                _logger.LogInformation($"[GetDetailProduct] Start to get product with id: {id}");
                var product = _productRepo.GetFirstOrNullAsync(id);

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
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get detail product");
            }
        }

        public BaseResponse UpdateProduct(int id, ProductRequest request)
        {
            try
            {
                _logger.LogInformation($"[UpdateProduct] Start to update product with id: {id}, " +
                    $"ProductName: {request.ProductName}, Price: {request.Price}");
                var productCheck = _productRepo.GetFirstOrNullAsync(id);

                if (productCheck == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                var product = _mapper.Map<Product>(request);

                product.ProductId = id;

                var recordChanges = _productRepo.UpdateAsync(product);

                if (recordChanges == 0)
                {
                    return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update");
                }

                return new BaseResponse("Update product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateProduct] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update product");
            }
        }
    }
}
