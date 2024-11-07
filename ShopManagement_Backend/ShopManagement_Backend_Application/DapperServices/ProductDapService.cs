using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        
        public ProductDapService(
            IMapper mapper,
            IProductDapRepository productRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
        }

        public BaseResponse CreateProduct(ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            product.IsDeleted = false;

            var recordChanges = _productRepo.AddAsync(product);

            if (recordChanges == 0)
            {
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create");
            }

            return new BaseResponse("Create product successfully");
        }

        public BaseResponse DeleteProduct(int id)
        {
            var product = _productRepo.GetFirstOrNullAsync(id);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            var recordChanges = _productRepo.DeleteAsync(product);

            if (recordChanges == 0)
            {
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete");
            }

            return new BaseResponse("Delte product successfully");
        }

        public BaseResponse GetAll()
        {
            var productList = _productRepo.GetAllAsync();

            var responseList = _mapper.Map<List<ProductResponse>>(productList);

            return new BaseResponse(responseList);
        }

        public BaseResponse GetDetailProduct(int id)
        {
            var product = _productRepo.GetFirstOrNullAsync(id);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            var response = _mapper.Map<ProductResponse>(product);

            return new BaseResponse(response);
        }

        public BaseResponse UpdateProduct(int id, ProductRequest request)
        {
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
    }
}
