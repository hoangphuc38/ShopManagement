using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        public ProductService(
            IMapper mapper,
            IProductRepository productRepo,
            IShopDetailRepository shopDetailRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _shopDetailRepo = shopDetailRepo;
        }

        public BaseResponse GetAll()
        {
            var productList = _productRepo.GetAllAsync(t => t.IsDeleted == false);
            var responseList = _mapper.Map<List<ProductResponse>>(productList);

            return new BaseResponse(responseList);
        }

        public BaseResponse GetDetailProduct(int id)
        {
            var product = _productRepo.GetFirstAsync(t => t.ProductId == id);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            var response = _mapper.Map<ProductResponse>(product);

            return new BaseResponse(response);
        }

        public BaseResponse UpdateProduct(int id, ProductRequest request)
        {
            var product = _productRepo.GetFirstAsync(t => t.ProductId == id);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            product.ProductName = request.ProductName;
            product.Price = request.Price;

            _productRepo.UpdateAsync(product);

            return new BaseResponse("Update product successfully");
        }

        public BaseResponse DeleteProduct(int id)
        {
            var product = _productRepo.GetFirstAsync(t => t.ProductId == id);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            product.IsDeleted = true;
            _productRepo.UpdateAsync(product);


            var detailList = _shopDetailRepo.GetAllAsync(c => c.ProductId == id);
            foreach (var detail in detailList)
            {
                detail.IsDeleted = true;
                _shopDetailRepo.UpdateAsync(detail);
            }

            return new BaseResponse("Delete product successfully");
        }

        public BaseResponse CreateProduct(ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            product.IsDeleted = false;

            _productRepo.AddAsync(product);

            return new BaseResponse("Create product successfully");
        }
    }
}
