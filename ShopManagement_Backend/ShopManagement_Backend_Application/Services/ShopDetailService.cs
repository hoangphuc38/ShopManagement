using AutoMapper;
using Microsoft.AspNetCore.Http;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_Application.Services
{
    public class ShopDetailService : IShopDetailService
    {
        private readonly IMapper _mapper;
        private readonly IShopDetailRepository _shopDetailRepo;
        private readonly IProductRepository _productRepo;
        private readonly IShopRepository _shopRepo;

        public ShopDetailService(
            IMapper mapper,
            IShopDetailRepository shopDetailRepo,
            IProductRepository productRepo,
            IShopRepository shopRepo)
        {
            _mapper = mapper;
            _shopDetailRepo = shopDetailRepo;
            _productRepo = productRepo;
            _shopRepo = shopRepo;
        }

        public BaseResponse GetAllOfShop(int id)
        {
            var shop = _shopRepo.GetFirstAsync(c => c.ShopId == id && c.IsDeleted == false);
            var productList = _shopDetailRepo.GetAllAsync(c => c.ShopId == id && c.IsDeleted == false);
            var responseList = new List<ShopDetailResponse>();

            foreach (var product in productList)
            {
                var productRes = _productRepo.GetFirstAsync(c => c.ProductId == product.ProductId && c.IsDeleted == false);

                if (productRes == null)
                {
                    continue;
                }

                var response = _mapper.Map<ShopDetailResponse>(product);

                responseList.Add(response);
            }

            return new BaseResponse(responseList);
        }

        public BaseResponse UpdateDetail(ShopDetailRequest request)
        {
            var product = _productRepo.GetFirstAsync(c => c.ProductId == request.ProductId && c.IsDeleted == false);

            var shop = _shopRepo.GetFirstAsync(c => c.ShopId == request.ShopId && c.IsDeleted == false);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            if (shop == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
            }

            var detail = _shopDetailRepo.GetFirstAsync(c => c.ProductId == request.ProductId
                                 && c.ShopId == request.ShopId
                                 && c.IsDeleted == false);

            if (detail == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not exist this detail");
            }

            detail.Quantity = request.Quantity;

            _shopDetailRepo.UpdateAsync(detail);

            return new BaseResponse("Update detail successfully");
        }

        public BaseResponse DeleteDetail(int shopID, int productID)
        {
            var detail = _shopDetailRepo.GetFirstAsync(c => c.ProductId == productID
                                 && c.ShopId == shopID
                                 && c.IsDeleted == false);

            if (detail == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not exist this detail");
            }

            detail.IsDeleted = true;

            _shopDetailRepo.DeleteAsync(detail);

            return new BaseResponse("Delete detail successfully");
        }

        public BaseResponse CreateDetail(ShopDetailRequest request)
        {
            var product = _productRepo.GetFirstAsync(c => c.ProductId == request.ProductId && c.IsDeleted == false);

            var shop = _shopRepo.GetFirstAsync(c => c.ShopId == request.ShopId && c.IsDeleted == false);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            if (shop == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
            }

            var detail = _shopDetailRepo.GetFirstOrNullAsync(c => c.ProductId == request.ProductId
                                 && c.ShopId == request.ShopId
                                 && c.IsDeleted == false);

            //If detail not exist, create new one
            if (detail == null)
            {
                var shopDetail = _mapper.Map<ShopDetail>(request);
                shopDetail.IsDeleted = false;

                _shopDetailRepo.AddAsync(shopDetail);

                return new BaseResponse("Create new detail successfully");
            }

            //If exist, add quantity for detail
            detail.Quantity += request.Quantity;

            _shopDetailRepo.UpdateAsync(detail);

            return new BaseResponse("Add quantity for detail because this detail has existed");
        }
    }
}
